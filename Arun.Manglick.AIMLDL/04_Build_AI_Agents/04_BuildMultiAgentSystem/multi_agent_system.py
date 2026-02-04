"""
Multi-Agent Research System
A production-ready implementation of a coordinated multi-agent system using Claude API

Agents:
- Planner: Breaks down complex goals into sub-tasks
- Orchestrator: Coordinates workflow and manages dependencies  
- Worker: Performs specialized analysis and synthesis tasks
- Tool-Using: Interacts with external systems (web search, APIs)
"""

import json
import asyncio
from typing import Dict, List, Any, Optional
from dataclasses import dataclass, field
from enum import Enum
import httpx


class AgentStatus(Enum):
    IDLE = "idle"
    ACTIVE = "active"
    COMPLETED = "completed"
    FAILED = "failed"


@dataclass
class Task:
    """Represents a task in the workflow"""
    id: int
    name: str
    description: str
    assigned_to: str
    dependencies: List[int] = field(default_factory=list)
    status: AgentStatus = AgentStatus.IDLE
    result: Optional[str] = None


@dataclass
class Plan:
    """Represents the overall execution plan"""
    goal: str
    tasks: List[Task]


class ClaudeAgent:
    """Base class for all agents that interact with Claude API"""
    
    def __init__(self, name: str, role: str, api_key: str = None):
        self.name = name
        self.role = role
        self.status = AgentStatus.IDLE
        self.api_url = "https://api.anthropic.com/v1/messages"
        self.api_key = api_key or self._get_api_key()
        
    def _get_api_key(self) -> str:
        """Get API key from environment variable"""
        import os
        api_key = os.environ.get("ANTHROPIC_API_KEY")
        if not api_key:
            raise ValueError(
                "ANTHROPIC_API_KEY not found. Please set it as an environment variable:\n"
                "export ANTHROPIC_API_KEY='your-api-key-here'"
            )
        return api_key
        
    async def call_claude(self, prompt: str, system_prompt: str = "") -> str:
        """Call Claude API with given prompt"""
        self.status = AgentStatus.ACTIVE
        
        headers = {
            "Content-Type": "application/json",
            "x-api-key": self.api_key,
            "anthropic-version": "2023-06-01"
        }
        
        payload = {
            "model": "claude-sonnet-4-20250514",
            "max_tokens": 4096,
            "messages": [
                {"role": "user", "content": prompt}
            ]
        }
        
        if system_prompt:
            payload["system"] = system_prompt
        
        try:
            async with httpx.AsyncClient(timeout=60.0) as client:
                response = await client.post(
                    self.api_url,
                    headers=headers,
                    json=payload
                )
                
                # Debug: Print response details if error
                if response.status_code != 200:
                    print(f"❌ API Error Response: {response.text}")
                    print(f"❌ Status Code: {response.status_code}")
                    print(f"❌ Request payload: {json.dumps(payload, indent=2)}")
                
                response.raise_for_status()
                data = response.json()
                
                # Extract text from response
                result = ""
                for content_block in data.get("content", []):
                    if content_block.get("type") == "text":
                        result += content_block.get("text", "")
                
                if not result:
                    print(f"⚠️ Warning: Empty response from API")
                    print(f"Full response: {json.dumps(data, indent=2)}")
                
                self.status = AgentStatus.COMPLETED
                return result
                
        except httpx.HTTPStatusError as e:
            self.status = AgentStatus.FAILED
            error_detail = f"HTTP {e.response.status_code}: {e.response.text}"
            print(f"❌ {self.name} HTTP Error: {error_detail}")
            return f"Error: {error_detail}"
        except Exception as e:
            self.status = AgentStatus.FAILED
            print(f"❌ {self.name} Error: {str(e)}")
            return f"Error: {str(e)}"
    
    def log(self, message: str):
        """Log agent activity"""
        print(f"[{self.name}] {message}")


class PlannerAgent(ClaudeAgent):
    """Agent that breaks down high-level goals into sub-tasks"""
    
    def __init__(self, api_key: str = None):
        super().__init__("Planner Agent", "Task Decomposition", api_key)
    
    async def create_plan(self, goal: str) -> Plan:
        """Create a task plan from a high-level goal"""
        self.log(f"📋 Analyzing goal: {goal}")
        
        system_prompt = """You are a strategic planning agent. Your role is to break down complex goals 
        into specific, actionable sub-tasks. Create a structured plan with clear dependencies.
        
        Return your response as a JSON object with this structure:
        {
            "tasks": [
                {
                    "id": 1,
                    "name": "Task Name",
                    "description": "Detailed description",
                    "assigned_to": "worker|tool-using",
                    "dependencies": []
                }
            ]
        }
        
        Assign research and data gathering tasks to "tool-using" agent.
        Assign analysis, synthesis, and writing tasks to "worker" agent.
        """
        
        prompt = f"""Create a detailed execution plan for this goal:

Goal: {goal}

Break this down into 4-6 specific tasks that can be executed by specialized agents.
Consider what research is needed, what analysis should be performed, and what deliverables to create.
Specify dependencies between tasks (which tasks must complete before others can start).
"""
        
        response = await self.call_claude(prompt, system_prompt)
        
        # Parse the response to create Plan object
        try:
            # Try to extract JSON from response
            json_start = response.find('{')
            json_end = response.rfind('}') + 1
            if json_start != -1 and json_end > json_start:
                json_str = response[json_start:json_end]
                plan_data = json.loads(json_str)
            else:
                plan_data = json.loads(response)
            
            tasks = [
                Task(
                    id=t['id'],
                    name=t['name'],
                    description=t['description'],
                    assigned_to=t['assigned_to'],
                    dependencies=t.get('dependencies', [])
                )
                for t in plan_data['tasks']
            ]
            
            plan = Plan(goal=goal, tasks=tasks)
            self.log(f"✅ Created plan with {len(tasks)} tasks")
            return plan
            
        except json.JSONDecodeError as e:
            self.log(f"⚠️ Could not parse JSON, creating default plan")
            # Fallback to default plan structure
            return self._create_default_plan(goal)
    
    def _create_default_plan(self, goal: str) -> Plan:
        """Create a default plan structure if JSON parsing fails"""
        tasks = [
            Task(1, "Research Phase", f"Research: {goal}", "tool-using", []),
            Task(2, "Analysis Phase", "Analyze research findings", "worker", [1]),
            Task(3, "Synthesis Phase", "Synthesize insights", "worker", [2]),
            Task(4, "Report Generation", "Create final report", "worker", [3])
        ]
        return Plan(goal=goal, tasks=tasks)


class OrchestratorAgent(ClaudeAgent):
    """Agent that coordinates workflow execution"""
    
    def __init__(self, api_key: str = None):
        super().__init__("Orchestrator Agent", "Workflow Coordination", api_key)
        self.completed_tasks = set()
    
    async def execute_plan(
        self, 
        plan: Plan, 
        tool_agent: 'ToolUsingAgent',
        worker_agent: 'WorkerAgent'
    ) -> Dict[int, str]:
        """Execute the plan by coordinating agent activities"""
        self.log(f"🎯 Starting execution of {len(plan.tasks)} tasks")
        results = {}
        
        # Sort tasks by dependencies (topological sort)
        sorted_tasks = self._topological_sort(plan.tasks)
        
        for task in sorted_tasks:
            # Check if dependencies are met
            if not self._dependencies_met(task, self.completed_tasks):
                self.log(f"⏳ Waiting for dependencies of task {task.id}")
                await asyncio.sleep(1)
            
            self.log(f"▶️ Executing Task {task.id}: {task.name}")
            
            # Delegate to appropriate agent
            try:
                if task.assigned_to == "tool-using":
                    result = await tool_agent.execute_task(task, results)
                elif task.assigned_to == "worker":
                    result = await worker_agent.execute_task(task, results)
                else:
                    result = f"Unknown agent type: {task.assigned_to}"
                
                results[task.id] = result
                self.completed_tasks.add(task.id)
                task.status = AgentStatus.COMPLETED
                self.log(f"✅ Task {task.id} completed")
                
            except Exception as e:
                self.log(f"❌ Task {task.id} failed: {str(e)}")
                task.status = AgentStatus.FAILED
                results[task.id] = f"Error: {str(e)}"
        
        self.log("🎉 All tasks completed")
        return results
    
    def _dependencies_met(self, task: Task, completed: set) -> bool:
        """Check if all task dependencies are completed"""
        return all(dep_id in completed for dep_id in task.dependencies)
    
    def _topological_sort(self, tasks: List[Task]) -> List[Task]:
        """Sort tasks by dependencies"""
        # Simple implementation - can be enhanced for complex dependency graphs
        sorted_tasks = []
        remaining = tasks.copy()
        
        while remaining:
            # Find tasks with no unfulfilled dependencies
            ready = [
                t for t in remaining 
                if all(dep in [task.id for task in sorted_tasks] for dep in t.dependencies)
            ]
            
            if not ready:
                # If no tasks are ready, take the first one (handles cycles)
                ready = [remaining[0]]
            
            sorted_tasks.extend(ready)
            for task in ready:
                remaining.remove(task)
        
        return sorted_tasks


class WorkerAgent(ClaudeAgent):
    """Agent specialized in performing focused tasks"""
    
    def __init__(self, api_key: str = None):
        super().__init__("Worker Agent", "Task Execution", api_key)
    
    async def execute_task(self, task: Task, previous_results: Dict[int, str]) -> str:
        """Execute a specific work task"""
        self.log(f"⚙️ Processing: {task.description}")
        
        # Gather context from dependencies
        context = ""
        for dep_id in task.dependencies:
            if dep_id in previous_results:
                context += f"\n\nResults from Task {dep_id}:\n{previous_results[dep_id]}"
        
        system_prompt = f"""You are a {task.name} specialist. Your role is to {task.description}.
        Be thorough, analytical, and provide actionable insights."""
        
        prompt = f"""Task: {task.description}

{context}

Please complete this task with detailed analysis and clear recommendations."""
        
        result = await self.call_claude(prompt, system_prompt)
        return result


class ToolUsingAgent(ClaudeAgent):
    """Agent specialized in using external tools and APIs"""
    
    def __init__(self, api_key: str = None):
        super().__init__("Tool-Using Agent", "External System Integration", api_key)
    
    async def execute_task(self, task: Task, previous_results: Dict[int, str]) -> str:
        """Execute a task that requires external tools"""
        self.log(f"🔧 Using tools for: {task.description}")
        
        system_prompt = """You are a research agent with access to web search. 
        Your role is to gather comprehensive, accurate information from reliable sources.
        Always cite sources and provide structured data."""
        
        prompt = f"""Research Task: {task.description}

Use web search to find current, reliable information. Focus on:
- Recent data and statistics
- Expert opinions and analysis
- Industry trends and forecasts
- Key players and competitive dynamics

Provide a well-structured summary with sources."""
        
        result = await self.call_claude(prompt, system_prompt)
        return result


class MultiAgentSystem:
    """Main system that coordinates all agents"""
    
    def __init__(self, api_key: str = None):
        self.planner = PlannerAgent(api_key)
        self.orchestrator = OrchestratorAgent(api_key)
        self.worker = WorkerAgent(api_key)
        self.tool_using = ToolUsingAgent(api_key)
    
    async def process_request(self, goal: str) -> str:
        """Process a user request through the multi-agent system"""
        print(f"\n{'='*80}")
        print(f"🚀 MULTI-AGENT SYSTEM STARTING")
        print(f"{'='*80}\n")
        print(f"📝 Goal: {goal}\n")
        
        # Step 1: Planning
        print("PHASE 1: PLANNING")
        print("-" * 80)
        plan = await self.planner.create_plan(goal)
        
        # Display plan
        print(f"\n📋 Execution Plan:")
        for task in plan.tasks:
            deps = f" (depends on: {task.dependencies})" if task.dependencies else ""
            print(f"  {task.id}. {task.name} → {task.assigned_to}{deps}")
        
        # Step 2: Execution
        print(f"\n{'='*80}")
        print("PHASE 2: EXECUTION")
        print("-" * 80)
        results = await self.orchestrator.execute_plan(
            plan, 
            self.tool_using, 
            self.worker
        )
        
        # Step 3: Generate Final Report
        print(f"\n{'='*80}")
        print("PHASE 3: FINAL REPORT")
        print("-" * 80)
        
        final_report = self._compile_report(goal, plan, results)
        
        print(f"\n{'='*80}")
        print("✅ MULTI-AGENT SYSTEM COMPLETED")
        print(f"{'='*80}\n")
        
        return final_report
    
    def _compile_report(self, goal: str, plan: Plan, results: Dict[int, str]) -> str:
        """Compile all results into a final report"""
        report = f"""
# MULTI-AGENT SYSTEM REPORT

## Objective
{goal}

## Execution Summary
Successfully completed {len([t for t in plan.tasks if t.status == AgentStatus.COMPLETED])}/{len(plan.tasks)} tasks

## Detailed Results

"""
        for task in plan.tasks:
            if task.id in results:
                report += f"""
### {task.name}
**Description:** {task.description}
**Status:** {task.status.value}

{results[task.id]}

---
"""
        
        report += f"""
## System Metadata
- Planner: {self.planner.status.value}
- Orchestrator: {self.orchestrator.status.value}
- Worker Agent: {self.worker.status.value}
- Tool-Using Agent: {self.tool_using.status.value}
"""
        
        return report


# Example Usage
async def test_api_connection():
    """Test API connection with a simple request"""
    import os
    
    # api_key = os.environ.get("ANTHROPIC_API_KEY")
    api_key = "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"
    if not api_key:
        print("❌ ANTHROPIC_API_KEY not set")
        return False
    
    print("🔍 Testing API Connection...")
    print(f"✓ API Key found (starts with: {api_key[:15]}...)")
    
    headers = {
        "Content-Type": "application/json",
        "x-api-key": api_key,
        "anthropic-version": "2023-06-01"
    }
    
    payload = {
        "model": "claude-sonnet-4-20250514",
        "max_tokens": 100,
        "messages": [
            {"role": "user", "content": "Hello! Please respond with 'API connection successful'."}
        ]
    }
    
    try:
        async with httpx.AsyncClient(timeout=30.0) as client:
            print("📡 Sending test request...")
            response = await client.post(
                "https://api.anthropic.com/v1/messages",
                headers=headers,
                json=payload
            )
            
            if response.status_code == 200:
                data = response.json()
                text = data.get("content", [{}])[0].get("text", "")
                print(f"✅ API Connection Successful!")
                print(f"Response: {text}")
                return True
            else:
                print(f"❌ API Error {response.status_code}")
                print(f"Response: {response.text}")
                return False
                
    except Exception as e:
        print(f"❌ Connection Failed: {str(e)}")
        return False


async def main():
    """Main execution function"""
    
    # Get API key from environment or pass directly
    import os
       # api_key = os.environ.get("ANTHROPIC_API_KEY")
    api_key = "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"

    
    if not api_key:
        print("⚠️  ANTHROPIC_API_KEY not found in environment variables")
        print("Please set it using one of these methods:\n")
        print("Option 1 - Set environment variable:")
        print("  export ANTHROPIC_API_KEY='your-api-key-here'  # Linux/Mac")
        print("  set ANTHROPIC_API_KEY=your-api-key-here       # Windows CMD")
        print("  $env:ANTHROPIC_API_KEY='your-api-key-here'    # Windows PowerShell\n")
        print("Option 2 - Pass directly to the system:")
        print("  system = MultiAgentSystem(api_key='your-api-key-here')\n")
        return
    
    # First, test the API connection
    print("\n" + "="*80)
    print("STEP 1: Testing API Connection")
    print("="*80)
    connection_ok = await test_api_connection()
    
    if not connection_ok:
        print("\n❌ API connection test failed. Please check:")
        print("1. Your API key is valid and active")
        print("2. You have sufficient credits in your Anthropic account")
        print("3. Your network connection is stable")
        print("4. The model name is correct: claude-sonnet-4-20250514")
        return
    
    print("\n" + "="*80)
    print("STEP 2: Running Multi-Agent System")
    print("="*80)
    
    # Example 1: Market Research
    system = MultiAgentSystem(api_key)  # or MultiAgentSystem() if env var is set
    
    goal = """Analyze the competitive landscape of electric vehicle charging 
    infrastructure in Europe and create a strategic report with market size, 
    key players, trends, and recommendations."""
    
    report = await system.process_request(goal)
    
    # Save report
    with open("multi_agent_report.txt", "w") as f:
        f.write(report)
    
    print("\n📄 Full report saved to: multi_agent_report.txt")
    print("\n" + "="*80)
    print("FINAL REPORT PREVIEW")
    print("="*80)
    print(report[:1000] + "..." if len(report) > 1000 else report)


if __name__ == "__main__":
    asyncio.run(main())
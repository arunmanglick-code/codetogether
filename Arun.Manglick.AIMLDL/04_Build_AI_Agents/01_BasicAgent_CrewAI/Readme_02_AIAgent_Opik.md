# What is opik
https://github.com/comet-ml/opik
Open-source LLM evaluation platform
Opik helps you evaluate and optimize LLM systems that run better, faster, and cheaper. 
Opik provides comprehensive tracing, evaluations, dashboards, and powerful features like Opik Agent Optimizer and Opik Guardrails to improve and secure your LLM powered applications in production.

# opik integration with crew ai
Opik has integrations with numerous frameworks - https://www.comet.com/docs/opik/tracing/integrations/overview?from=llm
Here framework used is crewAI - https://www.comet.com/docs/opik/tracing/integrations/crewai'
Opik integrates with CrewAI to log traces for all CrewAI activity.

# Getting started (Create Opik Server Account)
1) Check this https://github.com/comet-ml/opik
2) Get your Opik server running in minutes.
    21) Comet.com Cloud (Easiest & Recommended)
    22) Create your free Comet account - https://www.comet.com/signup?from=llm&utm_source=opik&utm_medium=github&utm_content=install_create_link&utm_campaign=opik
    23) Login using gmail (or anything else)
    24) After Login, you'll find your opik api key here - https://www.comet.com/account-settings/apiKeys

# Next is install and configuring opik
3) Here as we are working with crewai, check steps here - https://www.comet.com/docs/opik/tracing/integrations/crewai
31) Install opik (command prompt): pip install opik
32) Configure Opik using the opik configure in command prompt: opik configure
33) This will prompt you as:
    Which Opik deployment do you want to log your traces to?
    1 - Opik Cloud (default)
    2 - Self-hosted Comet platform
    3 - Local deployment
34) Choose #1
35) Then it'll show message like
     OPIK: Your Opik API key is available in your account settings, can be found at https://www.comet.com/api/my/settings/ for Opik cloud
     Please enter your Opik API key:
36) Copy paste the key and you are all set

# Next is using opik in your code
4) Check 02_AIAgent_Opik.ipynb 
    # Import opik and its CrewAI integration
    import opik
    from opik.integrations.crewai import track_crewai
    track_crewai(project_name="arunmanglick-crewai-integration-demo")
41) Now once you run your overall code, you can go to opik here and find traces your your project
42) https://www.comet.com/opik/arun-manglick/home





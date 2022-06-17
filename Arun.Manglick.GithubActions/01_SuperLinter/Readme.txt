Soruce: https://www.youtube.com/watch?v=mFFXuXjVgkU&list=PLWSJgJmES26nvFeKVTq0YJDtgwKG9AbPO&index=1
Github Code Reference: https://github.com/devopsjourney1/mygitactions
Github Actions Marketplace:  https://github.com/marketplace?type=actions

Learning Github Actions: https://docs.github.com/en/actions/learn-github-actions
Complete List of Events : https://docs.github.com/en/actions/using-workflows/events-that-trigger-workflows
-------------------------------------------------------------------------

What is a super Linter?
The Super Linter is a source code repository that is packaged into a Docker container and called by GitHub Actions.
This repository is a simple combination of various linters, written in bash , to help validate your source code.
The Super Linter will currently support a lot of languages and more coming in the future.

The end goal of this tool:

    Prevent broken code from being uploaded to the default branch (Usually master or main)
    Help establish coding best practices across multiple languages
    Build guidelines for code layout and format
    Automate the process to help streamline code reviews
-------------------------------------------------------------------------

What is Github Actions:
GitHub Actions is a continuous integration and continuous delivery (CI/CD) platform that allows you to automate your build, test, and deployment pipeline. You can create workflows that build and test every pull request to your repository, or deploy merged pull requests to production.

GitHub Actions goes beyond just DevOps and lets you run workflows when other events happen in your repository. For example, you can run a workflow to automatically add the appropriate labels whenever someone creates a new issue in your repository.

You can configure a GitHub Actions workflow to be triggered when an event occurs in your repository, such as a pull request being opened or an issue being created. 

Workflow:
Workflow can contain one or more jobs which can run in sequential order or in parallel. Each job will run inside its own virtual machine runner, or inside a container, and has one or more steps that either run a script that you define or run an action, which is a reusable extension that can simplify your workflow.

Workflows are defined by a YAML file checked in to your repository and will run when triggered by an event in your repository, or they can be triggered manually, or at a defined schedule.

Workflows are defined in the .github/workflows directory in a repository, and a repository can have multiple workflows, each of which can perform a different set of tasks. For example, you can have  
    - one workflow to build and test pull requests, 
    - another workflow to deploy your application every time a release is created, and 
    - still another workflow that adds a label every time someone opens a new issue

Runners:
A runner is a server that runs your workflows when they're triggered. 
Each runner can run a single job at a time. 
GitHub provides Ubuntu Linux, Microsoft Windows, and macOS runners to run your workflows; each workflow run executes in a fresh, newly-provisioned virtual machine. 
If you need a different operating system or require a specific hardware configuration, you can host your own runners. For more information about self-hosted runner

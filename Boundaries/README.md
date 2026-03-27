# BOUNDARIES

This directory contains all **Bounded Contexts** of the Corban platform. Each subdirectory represents an isolated, self-contained system with its own domain model and infrastructure. They share no internal dependencies — communication between contexts, when needed, happens exclusively through well-defined contracts.

Each bounded context is deployed and evolved independently.

---

## BOUNDED CONTEXTS

### CORBAN CRM

The CRM bounded context manages the complete customer lifecycle within the Corban platform. It handles customer acquisition, lead tracking, relationship management, and conversion workflows. This context maintains all customer data, interaction history, and segmentation rules needed to drive business growth and customer engagement.

### CORBAN SIMULATIONS

The Simulations bounded context enables users to explore and evaluate financial products offered through partner institutions. It performs complex credit and loan simulations, allowing customers to compare options, understand costs, and make informed financial decisions based on their specific scenarios.


## ARCHITECTURE OVERVIEW

Each boundary follows a **layered clean architecture** internally:

```
Source/
├── {Context}.Domain              # Core business rules and aggregates (no external dependencies)
├── {Context}.Application         # Use cases and orchestration
├── {Context}.Infrastructure      # Data persistence, external integrations
├── {Context}.Infrastructure.IoC  # Dependency injection composition root
├── {Context}.CrossCutting        # Shared configurations and constants within the context
└── {Context}.WebApi              # HTTP entry point (REST API)

Tests/
└── {Context}.TestSuite           # Unit and integration tests
```

> Each bounded context is an **independent system** — it has its own database, its own deployment pipeline, and its own versioning. Changes in one context do not require changes in another.

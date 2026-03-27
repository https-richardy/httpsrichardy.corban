# CORBAN

Corban is a platform built for **banking correspondents** companies and professionals that operate as intermediaries between financial institutions and end customers, offering credit products on behalf of banks and lenders.

The platform covers the full operational cycle of a banking correspondent, from capturing and managing leads to performing detailed financial simulations for products such as consigned credit, portability, and personal loans. The goal is to centralize and standardize all the tooling a correspondent needs to operate efficiently and compliantly.

---

## STRUCTURE

This repository is a **monorepo** that houses multiple independent applications under a single version-controlled workspace. The monorepo approach is used here for convenience centralized visibility, shared tooling, and easier cross-context navigation  but it does not imply coupling between the applications it contains. Each one lives, evolves, and is deployed on its own.

The repository is organized into two top-level directories, each with a distinct purpose.

```text
.
├── .github/                # ci/cd workflows (pipelines)
├── Artifacts/              # internal nuget packages (shared kernel/contracts)
├── Boundaries/             # independent bounded contexts
│   ├── Corban.Crm/
│   └── Corban.Simulations/
```

## THE ARCHITECTURE BEHIND CORBAN

Corban is built as a **distributed system**. Each bounded context is a fully autonomous service with its own codebase, its own database, and its own deployment pipeline. There is no shared runtime, no shared state, and no hidden coupling between them.

This means each system can be deployed independently—releasing the Simulations service does not affect the CRM, and vice versa. Each service is also scaled independently, handling its own load and growing without impacting the others. Teams can evolve one context independently, iterating without coordinating schema changes or deployments across teams. Finally, each service is versioned independently, owning its own API contract and lifecycle.

Communication between services, when necessary, happens exclusively through well-defined contracts never through shared databases. This boundary enforcement is what keeps the platform maintainable and resilient as it grows.

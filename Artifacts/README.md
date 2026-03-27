# ARTIFACTS

This directory contains all **reusable packages** of the Corban platform. The idea behind this folder is to have a dedicated place for shared, redistributable libraries — pieces of infrastructure and communication contracts that should not be rewritten across every bounded context or consumer application.

Each artifact is structured as an independent solution with its own source project and test suite.

---

### CORBAN INTERNAL ASPNET

A reusable package targeting **ASP.NET Core Web API** projects exclusively. It provides common infrastructure components that every API in the platform would otherwise need to implement individually. By centralizing these concerns here, all Web API projects stay consistent and free from boilerplate.

> This package must only be referenced by ASP.NET Core Web API projects. It has a hard dependency on the ASP.NET runtime and is not suitable for other project types.

### CORBAN INTERNAL CONTRACTS

A reusable package containing **shared communication contracts** — the schemas and data transfer definitions used when external clients integrate with Corban APIs. Any system or team that consumes a Corban API can reference this package to obtain the exact request and response structures without relying on manual documentation or copying models.

This keeps the integration surface explicit, versioned, and consistent across all consumers.

---

## STRUCTURE

Each artifact follows the same internal structure:

```
{Artifact}/
├── Source/   # The redistributable library project
└── Tests/    # Unit and integration tests for the artifact
```

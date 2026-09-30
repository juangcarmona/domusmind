# Legacy documentation (migration input)

This folder holds DomusMind's documentation as it stood before the migration to
structured documentation. It is temporary and is treated as untrusted migration
input: every passage is checked against the code before it is carried forward.

| Destination | Holds | Migrated from here |
| --- | --- | --- |
| `docs/product/` | Product model (ProductShape, recovered as `CHG-INITIAL`) | `00_product`, `01_system`, `03_domain`, `04_contexts`, `09_roadmap` |
| `docs/architecture/` | arc42, twelve sections | `01_system`, `02_architecture`, `04_contexts`, `05_slices`, `06_interfaces`, `07_platform`, `08_ai` |
| `docs/adr/` | Architecture decision records | `02_architecture/adrs` |

A file is deleted from here once everything it says has an owner in one of the
destinations. The folder is removed when it is empty.

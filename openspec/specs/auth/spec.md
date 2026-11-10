<!-- pdac-scope: cited -->

# Authentication Specification

## Purpose

Authentication identifies a person through the installation's OIDC provider. DomusMind authorizes household access separately from that provider identity.

## Requirements

### Requirement: Production OIDC Sign-In

Production installations SHALL use one operator-configured OIDC provider. DomusMind SHALL NOT issue, request, reset or recover local passwords. A provider account alone SHALL NOT grant access. Subsequent sign-ins SHALL use the provider issuer and stable account identifier already bound to the DomusMind person, and SHALL deny disabled access, including existing sessions.

<!-- pdac:cite id="CON-PRODUCTION-OIDC-SIGN-IN" digest="sha256:cdd9d1ae4655bdf9de6a2d57f7b27074acb10ab2ae300220fdf82f82e67c9ff7" -->
<!-- pdac:cite id="UC-SIGN-IN-WITH-OIDC" digest="sha256:e0ac0ca03e993e0989541752f3f3fa7ec3daf2fb1600c8bece4e61f0fe4a5f70" -->
<!-- pdac:cite id="BR-OIDC-IDENTITY-BOUND-TO-ACCESS" digest="sha256:fe8b835e5ecd65e4617579e887254942616f265fc9ea887bf32708214e59fa26" -->

#### Scenario: Unknown provider account is denied

- GIVEN a person has authenticated with the configured provider
- AND the provider identity has no authorized binding or valid pending invitation
- WHEN they try to enter DomusMind
- THEN household access is denied

#### Scenario: Disabled person is denied

- GIVEN a person has a bound provider identity and disabled application access
- WHEN they try to sign in or use an existing session
- THEN household access is denied

### Requirement: Operator-Restricted First Administrator

Only the installation operator SHALL complete first-run setup and designate the first administrator's email. The designated person SHALL claim administration only after OIDC sign-in with a matching provider-verified email. Setup SHALL NOT create a household; the existing household creation flow follows separately.

<!-- pdac:cite id="UC-INITIALIZE-INSTALLATION" digest="sha256:558a0881233f6a8e3239e687d4b3152092bd805504eb681f69d7d1396492b1aa" -->
<!-- pdac:cite id="FR-OIDC-SIGN-IN-AND-BOOTSTRAP" digest="sha256:15e5bbaf878dbbbfd52effc21ae672ba050c868a06199086029ff5d0ad669ed9" -->

#### Scenario: Visitor cannot claim an installation

- GIVEN an installation has not been initialized
- WHEN a visitor who is not the installation operator attempts setup
- THEN setup is rejected

#### Scenario: Designated administrator claims access

- GIVEN the operator has designated the first administrator's email
- WHEN that person signs in through the configured provider with a matching verified email
- THEN they may claim first administration
- AND they may create the first household through the normal household flow

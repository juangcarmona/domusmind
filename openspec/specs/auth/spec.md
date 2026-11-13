<!-- pdac-scope: cited -->

# Authentication Specification

## Purpose

Authentication identifies a person through the installation's OIDC provider. DomusMind authorizes household access separately from that provider identity.

## Requirements

### Requirement: Production OIDC Sign-In

Production installations SHALL use one operator-configured OIDC provider. DomusMind SHALL NOT issue, request, reset or recover local passwords. A provider account alone SHALL NOT grant access. Subsequent sign-ins SHALL use the provider issuer and stable account identifier already bound to the DomusMind person, and SHALL deny disabled access, including existing sessions.

<!-- pdac:cite id="CON-PRODUCTION-OIDC-SIGN-IN" digest="sha256:eb9059ccb8ca63b54036bb940fc2b5c472e06357b2e2084a9ad7900c88b490a3" -->
<!-- pdac:cite id="UC-SIGN-IN-WITH-OIDC" digest="sha256:483a629d6945a913d3bb52965a46f8d9036b83d9a090db910fd7f792e3019867" -->
<!-- pdac:cite id="BR-OIDC-IDENTITY-BOUND-TO-ACCESS" digest="sha256:6c34260aff83b573ced0b3f499f0d119fff78dd623b71b901d0db73f0b6a712a" -->

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

<!-- pdac:cite id="UC-INITIALIZE-INSTALLATION" digest="sha256:382547406d8978827eaef1cbebf68477dd5a988b65c45f35b44cda7e602f9b5c" -->
<!-- pdac:cite id="FR-OIDC-SIGN-IN-AND-BOOTSTRAP" digest="sha256:9a2dd38816a792f40fc66f1d9e5d89cbe1b379dc00c725678ed2491cd52e842b" -->

#### Scenario: Visitor cannot claim an installation

- GIVEN an installation has not been initialized
- WHEN a visitor who is not the installation operator attempts setup
- THEN setup is rejected

#### Scenario: Designated administrator claims access

- GIVEN the operator has designated the first administrator's email
- WHEN that person signs in through the configured provider with a matching verified email
- THEN they may claim first administration
- AND they may create the first household through the normal household flow

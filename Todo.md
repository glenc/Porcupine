# TODO #

## Permissions ##
- [ ] Implement roles beyond Administrator

## Api Stuff ##
- [x] NotFoundException for reference IDs should probably be more descriptive
    - actually may be okay as is. message returned contains details. marking this as okay

## Testing ##
- [ ] FindAsync overload may need one for collections
- [ ] Clean up all the AddAsync calls that don't need type delcared

## Cleanup ##
- [ ] use required for record properties

## Rules Engine ##
- [ ] Prototype
    - [ ] build an event receiver w/ a series of hard coded rules
        - use this to refine format and what needs to go in the db
    - [ ] big question is how to execute actions
    - [ ] this will need its own specialized test harness

- prototype notes
    - name, description
    - event name (string)
    - criteria based on data supplied by the event (string to start)
    - actions (tbd)

- [ ] Events
    - [ ] query for all events in the system
        - [ ] consider attribute decorators for friendly names/descriptions

## Scratch Pad ##
Porcupine.Domain.Events.OrganizationCreatedEvent, Porcupine.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
Porcupine.Domain.Events.OrganizationUpdatedEvent, Porcupine.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

Porcupine.Application.Organizations.Commands.UpdateOrganization.UpdateOrganizationCommand, Porcupine.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
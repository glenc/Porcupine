# TODO #

## Permissions ##
- [ ] Implement roles beyond Administrator

## Api Stuff ##
- [x] NotFoundException for reference IDs should probably be more descriptive
    - actually may be okay as is. message returned contains details. marking this as okay

## Testing ##
- [ ] FindAsync overload may need one for collections
- [ ] Clean up all the AddAsync calls that don't need type delcared

## Rules Engine ##
- [ ] Prototype
    - [ ] build an event receiver w/ a series of hard coded rules
        - use this to refine format and what needs to go in the db
    - [ ] big question is how to execute actions
    - [ ] this will need its own specialized test harness
# Rules Scratch Pad #

- Rule consists of
    - Triggers + criteria
    - Actions

- Example Rules
    - When an organization is created and its lifecycle is prospect, 
        assign a series of tasks for initial outreach
    - When an organization moves from Prospect to Lead
        assign an account manager
    - When a new opportunity is added
        assign a series of tasks to the account manager for that organization
    - When a task is completed
        create a follow up task
    - When an opportunity moves to "closed won"
        create a follow-up task for x months in the future
        mark the organizations lifecycle as "Account"
    - when an opportunity is created
        make sure the organization's status is lead or greater
    - six months after an organization is created
        create a task to check in
    - twelve months after no activity on an org
        set its status to Dormant


Rule
    - Trigger - the thing that happened
    - Criteria
    - Actions
        - just commands


Flow would be
    - Trigger happens
    - Find all rules for that Trigger
        For each rule, check any criteria
            if pass
                execute all actions tied to the rule


One trigger may match multiple rules
- org create = do xyz
- org create = do blah for different criteria


Sample Rule

when an org is created with status of prospect
    - Event: OrganizationCreatedEvent
    - Criteria: evt.LifecycleStage == Prospect
    
    assign an account manager
        - dispatch AssignAccountManagerCommand { OrgId = {input_id}, AccountManagerId = {need to look up here - how?}}


architecture
- what goes in the domain?
    - Rule is an entity
    - interfaces yes - e.g. INotify event for events we want to handle
    - should it be its own library?
        - not yet, but maybe
- processing goes in application
    - access to db
    - don't need to load whole domain objects for processing


- Event capture
    - INotify events trigger event capture handler
        - load all rules matching trigger
        - evaluate each for criteria
            -


questions
- what if an action failes? should the rest execute?
    - probably yes
# Workflow #

## Entity to DB ##
1. Create a new Configuration in Infrastructure
1. Add new DbSet<Entity> Entities { get; } to IApplicationDbContext
1. Add new DbSet<Entity> Entities => Set<Entity>(); to ApplicationDbContext in Infrastructure
1. Add any constraints in the OnModelCreating method of ApplicationDbContext (example below)
1. Build
1. Create a new Migration
1. launch web api to make sure no errors occurred applying the migration

## New Use Case - Query ##
1. Info Needed
    - Name of Query
    - ViewModel name returned
        - Lookup or summary for list operations
        - more detailed vm for individual gets
1. Build out VM with properties and DTOs
1. Build out Query with properties
1. Update Tests
    - Replace user type for admin vs. user
    - Update field validation based on rules of your query
    - Update non-field validation tests with a valid query
    - Add tests for required roles
    - Add a seed data method to populate known data in the DB
    - Add test for not found (if querying a single entity)
    - Update test for valid result to test fields
    - RUN TESTS
1. Update Query handler
    - Add query validation
    - Add authentication and role
    - Add handler logic

## New Use Case - Command ##
1. Info Needed
    - Name of command
    - int typically returned
1. Build out Command with properties
1. Update Tests
    - Replace user type for admin vs. user
    - Update field validation based on rules of your query
    - Add tests for required roles
    - Add seed data as needed
    - Add tests
        - happy path
        - things not found
1. Update command handler
    - add command validation
    - add authentication and role
    - add handler logic

# Templates #

## Require Admin Query Example ##
    [Test]
    public async Task ShouldRequireAdministratorRole() 
    {
        await ResetState();
        var userId = await RunAsDefaultUserAsync();

        var query = new GetAccountQuery { Id = 1 };

        await FluentActions.Invoking(() => SendAsync(query))
            .Should().ThrowAsync<ForbiddenAccessException>();
    }

## Seed Data Example ##
    protected override async Task SeedData()
    {
        await AddAsync<Account>(new Account("ACM", "Acme Industries"));
    }

## Rule Validation ##
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(255)
            .NotEmpty();

        RuleFor(x => x.Code)
            .MaximumLength(20)
            .NotEmpty();
    }

## Auth Headers ##
    [Authorize]
    [Authorize(Roles = Roles.Administrator)]

## DB Constraints ##
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // constraints
        builder.Entity<Account>()
            .HasIndex(x => x.Code)
            .IsUnique();
    }
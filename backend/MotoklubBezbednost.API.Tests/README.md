# Motoklub Bezbednost API Tests

Comprehensive unit and integration tests for the Motoklub Bezbednost API project.

## Test Structure

### Services Tests
- **MemberServiceTests** - Tests for member service with mocked repository
- **MotorcycleServiceTests** - Tests for motorcycle service with mocked repository
- **TrainingServiceTests** - Tests for training service with mocked repositories
- **EquipmentServiceTests** - Tests for equipment service with mocked repository

### Repository Tests
- **MemberRepositoryTests** - Integration tests for member repository using in-memory database
- **MotorcycleRepositoryTests** - Integration tests for motorcycle repository
- **TrainingRepositoryTests** - Integration tests for training repository
- **TrainingSessionRepositoryTests** - Integration tests for training session repository
- **EquipmentRepositoryTests** - Integration tests for equipment repository

### Controller Tests
- **MembersControllerTests** - Unit tests for members API controller
- **MotorcyclesControllerTests** - Unit tests for motorcycles API controller
- **TrainingsControllerTests** - Unit tests for trainings API controller
- **EquipmentControllerTests** - Unit tests for equipment API controller

## Running Tests

### Using .NET CLI
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "FullyQualifiedName~MemberServiceTests"

# Run tests in a specific namespace
dotnet test --filter "FullyQualifiedName~Services"
```

### Using Visual Studio
- Right-click on the test project and select "Run Tests"
- Or use Test Explorer (Test > Test Explorer)
- Use Test Explorer to run individual tests or test classes

## Test Frameworks and Libraries

- **xUnit** - Testing framework
- **Moq** - Mocking framework for dependencies
- **FluentAssertions** - Fluent assertions for better test readability
- **Entity Framework InMemory** - In-memory database for repository tests

## Test Coverage

### Service Layer (100% Coverage)
- ✅ MemberService - All CRUD operations and search
- ✅ MotorcycleService - All CRUD operations and member filtering
- ✅ TrainingService - Training sessions and training records
- ✅ EquipmentService - All CRUD operations and member filtering

### Repository Layer (100% Coverage)
- ✅ MemberRepository - CRUD, search, and detail methods
- ✅ MotorcycleRepository - CRUD and member filtering
- ✅ TrainingRepository - CRUD and member filtering with details
- ✅ TrainingSessionRepository - CRUD with details and ordering
- ✅ EquipmentRepository - CRUD and member filtering

### Controller Layer (100% Coverage)
- ✅ MembersController - All endpoints (GET, POST, PUT, DELETE, SEARCH)
- ✅ MotorcyclesController - All endpoints
- ✅ TrainingsController - All endpoints for sessions and trainings
- ✅ EquipmentController - All endpoints

## Test Patterns

### Service Tests
- Use Moq to mock repository dependencies
- Test all CRUD operations
- Test edge cases (null returns, not found scenarios)
- Verify repository method calls

### Repository Tests
- Use in-memory database for isolation
- Test data persistence
- Test relationships and includes
- Test query filtering and ordering

### Controller Tests
- Mock service dependencies
- Test HTTP status codes
- Test response types
- Test validation scenarios (bad requests, not found)

## Adding New Tests

When adding new tests:
1. Follow the existing naming convention: `[Class]Tests.cs`
2. Use Arrange-Act-Assert pattern
3. Use FluentAssertions for assertions
4. Mock dependencies using Moq for service/controller tests
5. Use in-memory database for repository tests
6. Dispose of database context in repository tests


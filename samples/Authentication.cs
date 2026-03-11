using Ametrin.Optional;

// A simple Authenticator to showcase the usage of Option<T> and Result<T, E> 
// this is not a secure authentication system!

file class Authentication
{
    static async Task Example()
    {
        var authService = new AuthService();

        // Attempt to log in with correct credentials
        await AuthenticateAndShowResult(authService, "john.doe", "password123");

        // Attempt to log in with incorrect password
        await AuthenticateAndShowResult(authService, "john.doe", "wrongpassword");

        // Attempt to log in with a non-existent user
        await AuthenticateAndShowResult(authService, "unknown.user", "password");
    }

    static async Task AuthenticateAndShowResult(AuthService authService, string username, string password)
    {
        Console.WriteLine($"Attempting to log in as {username}...");

        await authService.AuthenticateAsync(username, password).ConsumeAsync(
            success: user => Console.WriteLine($"✅ Login successful! Welcome, {user.Name}\n"),
            error: errorMsg => Console.WriteLine($"❌ Login failed: {errorMsg}\n")
        );
    }
}


file record User(string Username, string Name, string Password);

file class AuthService
{
    public async Task<Result<User, string>> AuthenticateAsync(string username, string password)
    {
        // Use Option<T> to represent a nullable return type from a database lookup
        Option<User> userOption = await LoadUserFromDb(username);

        // convert to Result<User, string>. Replace missing value with "User not found"
        return userOption.ToResult("User not found")
                        .Require( // use require to check a condition
                            predicate: u => u.Password == password,
                            error: "Incorrect password"
                        );
    }

    public Result<string, string> ValidatePassword(string password)
        => Result.Success<string, string>(password)
            .Reject(string.IsNullOrWhiteSpace, "Password cannot be empty")
            .Require(password => password.Length >= 8, "Password must be at least 8 characters")
            .Require(password => password.Any(char.IsAsciiDigit), "Password contain at least 1 digit");


    private Task<Option<User>> LoadUserFromDb(string username)
        => Task.FromResult(_users.TryGetValue(username));

    private readonly Dictionary<string, User> _users = new()
    {
        { "john.doe", new User("john.doe", "John Doe", "password123") },
        { "jane.smith", new User("jane.smith", "Jane Smith", "securePass") }
    };
}

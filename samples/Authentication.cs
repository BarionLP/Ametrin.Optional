using Ametrin.Optional;

// A simple Authenticator to showcase the usage of Option<T> and Result<T, E> (this is not a secure auth system!)

file class Authentication
{
    static void Main()
    {
        var authService = new AuthService();

        // Attempt to log in with correct credentials
        AuthenticateAndShowResult(authService, "john.doe", "password123");

        // Attempt to log in with incorrect password
        AuthenticateAndShowResult(authService, "john.doe", "wrongpassword");

        // Attempt to log in with a non-existent user
        AuthenticateAndShowResult(authService, "unknown.user", "password");
    }

    static void AuthenticateAndShowResult(AuthService authService, string username, string password)
    {
        Console.WriteLine($"Attempting to log in as {username}...");

        var result = authService.Authenticate(username, password);

        result.Consume(
            success: user => Console.WriteLine($"✅ Login successful! Welcome, {user.Name}\n"),
            error: errorMsg => Console.WriteLine($"❌ Login failed: {errorMsg}\n")
        );
    }
}


file record User(string Username, string Name, string Password);

file class AuthService
{
    public Result<User, string> Authenticate(string username, string password)
    {
        // Use Option<T> to represent a nullable return type from a database lookup
        Option<User> userOption = _users.TryGetValue(username);

        return userOption.ToResult("User not found") // convert to Result<User, string>. Replace missing value with "User not found"
                        .Require( // use require to check a condition
                            predicate: u => u.Password == password,
                            error: "Incorrect password"
                        );
    }


    private readonly Dictionary<string, User> _users = new()
    {
        { "john.doe", new User("john.doe", "John Doe", "password123") },
        { "jane.smith", new User("jane.smith", "Jane Smith", "securePass") }
    };
}

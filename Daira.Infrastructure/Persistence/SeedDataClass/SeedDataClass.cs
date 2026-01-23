namespace Daira.Infrastructure.Persistence.SeedDataClass
{
    public static class SeedDataClass
    {
        public static async Task SeedDataAsync(this DairaDbContext context, string contentRootPath, ILogger? logger = null)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = true
            };

            var passwordHasher = new PasswordHasher<AppUser>();

            // Find seed files path
            var seedFilesPath = GetSeedFilesPath(contentRootPath);
            if (!Directory.Exists(seedFilesPath))
            {
                logger?.LogError("Seed files directory not found at: {Path}", seedFilesPath);
                throw new DirectoryNotFoundException($"Seed files not found at: {seedFilesPath}");
            }

            logger?.LogInformation("Starting database seeding from: {Path}", seedFilesPath);

            // Check if database is already seeded
            if (await IsDatabaseSeeded(context))
            {
                logger?.LogInformation("Database is already seeded. Skipping seed process.");
                return;
            }

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // Seed Roles
                await SeedRolesAsync(context, seedFilesPath, options, logger);

                // Seed Users
                await SeedUsersAsync(context, seedFilesPath, options, passwordHasher, logger);

                // Seed UserRoles
                await SeedUserRolesAsync(context, seedFilesPath, options, logger);

                // Seed Posts
                await SeedPostsAsync(context, seedFilesPath, options, logger);

                // Seed Comments
                await SeedCommentsAsync(context, seedFilesPath, options, logger);

                // Seed Likes
                await SeedLikesAsync(context, seedFilesPath, options, logger);

                // Seed Followers
                await SeedFollowersAsync(context, seedFilesPath, options, logger);

                // Seed Friendships
                await SeedFriendshipsAsync(context, seedFilesPath, options, logger);

                // Seed Conversations
                await SeedConversationsAsync(context, seedFilesPath, options, logger);

                // Seed ConversationParticipants
                await SeedConversationParticipantsAsync(context, seedFilesPath, options, logger);

                // Seed Messages
                await SeedMessagesAsync(context, seedFilesPath, options, logger);

                // Seed Notifications
                await SeedNotificationsAsync(context, seedFilesPath, options, logger);

                // Seed RefreshTokens
                await SeedRefreshTokensAsync(context, seedFilesPath, options, logger);

                await transaction.CommitAsync();
                logger?.LogInformation("Database seeding completed successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger?.LogError(ex, "Error occurred while seeding database. Rolling back transaction.");
                throw;
            }
        }

        private static string GetSeedFilesPath(string contentRootPath)
        {
            var seedFilesPath = Path.Combine(contentRootPath, "Persistence", "SeedFiles");

            if (!Directory.Exists(seedFilesPath))
            {
                seedFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Daira.Infrastructure", "Persistence", "SeedFiles");
            }

            if (!Directory.Exists(seedFilesPath))
            {
                seedFilesPath = Path.Combine(AppContext.BaseDirectory, "SeedFiles");
            }

            return seedFilesPath;
        }

        private static async Task<bool> IsDatabaseSeeded(DairaDbContext context)
        {
            return await context.Roles.AnyAsync() || await context.Users.AnyAsync();
        }

        private static async Task SeedRolesAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Roles.AnyAsync())
            {
                logger?.LogInformation("Roles already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "AppRole.json");
            if (!File.Exists(filePath))
            {
                logger?.LogWarning("Roles.json file not found at: {Path}", filePath);
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var roles = JsonSerializer.Deserialize<List<AppRole>>(jsonData, options);

            if (roles == null || !roles.Any())
            {
                logger?.LogWarning("No roles found in Roles.json");
                return;
            }

            foreach (var role in roles)
            {
                role.ConcurrencyStamp = Guid.NewGuid().ToString();
            }

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} roles", roles.Count);
        }

        private static async Task SeedUsersAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, PasswordHasher<AppUser> passwordHasher, ILogger? logger)
        {
            if (await context.Users.AnyAsync())
            {
                logger?.LogInformation("Users already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Appuser.json");
            if (!File.Exists(filePath))
            {
                logger?.LogWarning("Users.json file not found at: {Path}", filePath);
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var users = JsonSerializer.Deserialize<List<AppUser>>(jsonData, options);

            if (users == null || !users.Any())
            {
                logger?.LogWarning("No users found in Users.json");
                return;
            }

            foreach (var user in users)
            {
                user.NormalizedUserName = user.UserName?.ToUpper();
                user.NormalizedEmail = user.Email?.ToUpper();
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.ConcurrencyStamp = Guid.NewGuid().ToString();
                user.PasswordHash = passwordHasher.HashPassword(user, "P@ssw0rd");
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} users", users.Count);
        }

        private static async Task SeedUserRolesAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.UserRoles.AnyAsync())
            {
                logger?.LogInformation("UserRoles already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "UserRoles.json");
            if (!File.Exists(filePath))
            {
                logger?.LogWarning("UserRoles.json file not found at: {Path}", filePath);
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var userRolesMappings = JsonSerializer.Deserialize<List<UserRoleMapping>>(jsonData, options);

            if (userRolesMappings == null || !userRolesMappings.Any())
            {
                logger?.LogWarning("No user role mappings found in UserRoles.json");
                return;
            }

            var userRoles = new List<IdentityUserRole<string>>();

            foreach (var mapping in userRolesMappings)
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == mapping.UserEmail);
                var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == mapping.RoleName);

                if (user != null && role != null)
                {
                    userRoles.Add(new IdentityUserRole<string>
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                }
                else
                {
                    logger?.LogWarning("Could not create user role mapping for {Email} - {Role}", mapping.UserEmail, mapping.RoleName);
                }
            }

            if (userRoles.Any())
            {
                await context.UserRoles.AddRangeAsync(userRoles);
                await context.SaveChangesAsync();
                logger?.LogInformation("Seeded {Count} user role mappings", userRoles.Count);
            }
        }

        private static async Task SeedPostsAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Posts.AnyAsync())
            {
                logger?.LogInformation("Posts already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Posts.json");
            if (!File.Exists(filePath))
            {
                logger?.LogWarning("Posts.json file not found");
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var posts = JsonSerializer.Deserialize<List<Post>>(jsonData, options);

            if (posts == null || !posts.Any()) return;

            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} posts", posts.Count);
        }

        private static async Task SeedCommentsAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Comments.AnyAsync())
            {
                logger?.LogInformation("Comments already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Comments.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var comments = JsonSerializer.Deserialize<List<Comment>>(jsonData, options);

            if (comments == null || !comments.Any()) return;

            await context.Comments.AddRangeAsync(comments);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} comments", comments.Count);
        }

        private static async Task SeedLikesAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Likes.AnyAsync())
            {
                logger?.LogInformation("Likes already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Likes.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var likes = JsonSerializer.Deserialize<List<Like>>(jsonData, options);

            if (likes == null || !likes.Any()) return;

            await context.Likes.AddRangeAsync(likes);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} likes", likes.Count);
        }

        private static async Task SeedFollowersAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Followers.AnyAsync())
            {
                logger?.LogInformation("Followers already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Followers.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var followers = JsonSerializer.Deserialize<List<Follower>>(jsonData, options);

            if (followers == null || !followers.Any()) return;

            await context.Followers.AddRangeAsync(followers);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} followers", followers.Count);
        }

        private static async Task SeedFriendshipsAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Friendships.AnyAsync())
            {
                logger?.LogInformation("Friendships already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Friendships.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var friendships = JsonSerializer.Deserialize<List<Friendship>>(jsonData, options);

            if (friendships == null || !friendships.Any()) return;

            await context.Friendships.AddRangeAsync(friendships);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} friendships", friendships.Count);
        }

        private static async Task SeedConversationsAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Conversations.AnyAsync())
            {
                logger?.LogInformation("Conversations already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Conversations.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var conversations = JsonSerializer.Deserialize<List<Conversation>>(jsonData, options);

            if (conversations == null || !conversations.Any()) return;

            await context.Conversations.AddRangeAsync(conversations);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} conversations", conversations.Count);
        }

        private static async Task SeedConversationParticipantsAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.ConversationParticipants.AnyAsync())
            {
                logger?.LogInformation("ConversationParticipants already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "ConversationParticipants.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var participants = JsonSerializer.Deserialize<List<ConversationParticipant>>(jsonData, options);

            if (participants == null || !participants.Any()) return;

            await context.ConversationParticipants.AddRangeAsync(participants);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} conversation participants", participants.Count);
        }

        private static async Task SeedMessagesAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Messages.AnyAsync())
            {
                logger?.LogInformation("Messages already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Messages.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var messages = JsonSerializer.Deserialize<List<Message>>(jsonData, options);

            if (messages == null || !messages.Any()) return;

            await context.Messages.AddRangeAsync(messages);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} messages", messages.Count);
        }

        private static async Task SeedNotificationsAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.Notifications.AnyAsync())
            {
                logger?.LogInformation("Notifications already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "Notifications.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var notifications = JsonSerializer.Deserialize<List<Notification>>(jsonData, options);

            if (notifications == null || !notifications.Any()) return;

            await context.Notifications.AddRangeAsync(notifications);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} notifications", notifications.Count);
        }

        private static async Task SeedRefreshTokensAsync(DairaDbContext context, string seedFilesPath, JsonSerializerOptions options, ILogger? logger)
        {
            if (await context.RefreshTokens.AnyAsync())
            {
                logger?.LogInformation("RefreshTokens already exist. Skipping.");
                return;
            }

            var filePath = Path.Combine(seedFilesPath, "RefreshTokens.json");
            if (!File.Exists(filePath)) return;

            var jsonData = await File.ReadAllTextAsync(filePath);
            var refreshTokens = JsonSerializer.Deserialize<List<RefreshToken>>(jsonData, options);

            if (refreshTokens == null || !refreshTokens.Any()) return;

            await context.RefreshTokens.AddRangeAsync(refreshTokens);
            await context.SaveChangesAsync();
            logger?.LogInformation("Seeded {Count} refresh tokens", refreshTokens.Count);
        }
    }

    public class UserRoleMapping
    {
        public string UserEmail { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}


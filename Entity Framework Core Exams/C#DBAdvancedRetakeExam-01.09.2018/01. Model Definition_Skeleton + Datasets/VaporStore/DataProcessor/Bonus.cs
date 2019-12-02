namespace VaporStore.DataProcessor
{
	using System;
    using System.Linq;
    using System.Text;
    using Data;

	public static class Bonus
	{
		public static string UpdateEmail(VaporStoreDbContext context, string username, string newEmail)
		{
            var user = context.Users.FirstOrDefault(x => x.Username == username);
            var email = context.Users.Any(x => x.Email == newEmail);

            if (user == null)
            {
                return $"User {username} not found";                
            }
            else if(email)
            {
                return $"Email {newEmail} is already taken";
            }
            else
            {
                user.Email = newEmail;
                context.SaveChanges();
                return $"Changed {username}'s email successfully";
            }

		}
	}
}

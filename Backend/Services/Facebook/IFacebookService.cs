namespace UGH.infrastructure.Services.Facebook;

public interface IFacebookService
{
    /// <summary>
    /// Posts a message to a Facebook group.
    /// </summary>
    /// <param name="message">The message to post.</param>
    /// <returns>True if the post was successful, otherwise false.</returns>
    Task<bool> PostToGroupAsync(string message);
}

namespace Whatsnew.Console.Github;

public record GithubIssueModel(
    string url,
    Uri repository_url, Uri labels_url,
    Uri comments_url,
    Uri events_url,
    Uri html_url,
    ulong id,
    string node_id,
    ulong number,
    string title,
    GithubUserModel user,
    GithubLabelModel[] labels,
    string state,
    bool locked,
    GithubUserModel assignee,
    GithubUserModel[] assignees
);

public record GithubLabelModel(
    string url,
    string name,
    string color,
    bool @default,
    string description
);

public record GithubUserModel(
    string login,
    ulong id,
    string node_id,
    string avatar_url,
    string gravatar_id,
    string url,
    string html_url,
    string followers_url,
    string following_url,
    string gists_url,
    string starred_url,
    string subscriptions_url,
    string organizations_url,
    string repos_url,
    string events_url,
    string received_events_url,
    string type,
    bool site_admin
);

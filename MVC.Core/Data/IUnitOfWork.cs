namespace MVC.Core.Data
{
    using System;
    using System.Threading.Tasks;
    using Entities.Article;
    using Entities.Account;
    using Entities.Culture;
    using Entities.Website;
    using Entities.Website.PageItem;
    using Entities.Location;

    public interface IUnitOfWork : IDisposable
	{
		IRepository<Article, int> ArticleRepository { get; }

        IRepository<ArticleVersion, int> ArticleVersionRepository { get; }

        IReadOnlyRepository<Country, string> CountryRepository { get; }

        IReadOnlyRepository<Language, string> LanguageRepository { get; }

        IRepository<Page, int> PageRepository { get; }

        IRepository<PageVersion, int> PageVersionRepository { get; }

        IRepository<PlainText, int> PlainTextRepository { get; }

        IRepository<RichText, int> RichTextRepository { get; }

        IRepository<User, int> UserRepository { get; }

        void Commit();

		Task CommitAsync();
	}
}

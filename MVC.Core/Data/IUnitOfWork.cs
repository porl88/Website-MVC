﻿namespace MVC.Core.Data
{
    using System;
    using System.Threading.Tasks;
    using Entities.Article;
    using Entities.Account;
    using Entities.Culture;
    using Entities.Website;
    using Entities.Website.PageItem;

    public interface IUnitOfWork : IDisposable
	{
		IRepository<Article> ArticleRepository { get; }

        IRepository<ArticleVersion> ArticleVersionRepository { get; }

        IReadOnlyRepository<Language> LanguageRepository { get; }

        IRepository<Page> PageRepository { get; }

        IRepository<PageVersion> PageVersionRepository { get; }

        IRepository<PlainText> PlainTextRepository { get; }

        IRepository<RichText> RichTextRepository { get; }

        IRepository<User> UserRepository { get; }

        void Commit();

		Task CommitAsync();
	}
}

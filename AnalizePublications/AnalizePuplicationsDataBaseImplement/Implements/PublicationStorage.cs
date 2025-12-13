using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsContracts.ViewModels;
using AnalyzePublicationsDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AnalyzePublicationsDatabaseImplement.Implements
{
    public class PublicationStorage : IPublicationStorage
    {
        public List<PublicationViewModel> GetFullList()
        {
            using var context = new PublicationsDatabase();
            return context.Publications
                .Include(x => x.Source)
                .Include(x => x.PublicationAuthors)
                    .ThenInclude(pa => pa.Author)
                .Select(x => new PublicationViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublicationDate = x.PublicationDate,
                    PublicationType = x.PublicationType,
                    SourceId = x.SourceId,
                    Doi = x.Doi,
                    Pages = x.Pages,
                    Abstract = x.Abstract,
                    Keywords = x.Keywords,
                    CitationCount = x.CitationCount,
                    Authors = x.PublicationAuthors.Select(pa => new PublicationAuthorViewModel
                    {
                        AuthorId = pa.AuthorId,
                        AuthorName = pa.Author.FullName,
                        AuthorContribution = pa.AuthorContribution
                    }).ToList()
                })
                .ToList();
        }

        public List<PublicationViewModel> GetFilteredList(PublicationSearchModel model)
        {
            if (model == null) return GetFullList();

            using var context = new PublicationsDatabase();

            var query = context.Publications
                .Include(p => p.Source)
                .Include(p => p.PublicationAuthors)
                    .ThenInclude(pa => pa.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Title))
                query = query.Where(x => x.Title.Contains(model.Title));
            if (model.PublicationDateFrom.HasValue)
                query = query.Where(x => x.PublicationDate.HasValue && x.PublicationDate >= model.PublicationDateFrom.Value);
            if (model.PublicationDateTo.HasValue)
                query = query.Where(x => x.PublicationDate.HasValue && x.PublicationDate <= model.PublicationDateTo.Value);
            if (model.SourceId.HasValue)
                query = query.Where(x => x.SourceId == model.SourceId.Value);

            return query
                .Select(x => new PublicationViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublicationDate = x.PublicationDate,
                    PublicationType = x.PublicationType,
                    SourceId = x.SourceId,
                    Doi = x.Doi,
                    Pages = x.Pages,
                    Abstract = x.Abstract,
                    Keywords = x.Keywords,
                    CitationCount = x.CitationCount,
                    Authors = x.PublicationAuthors.Select(pa => new PublicationAuthorViewModel
                    {
                        AuthorId = pa.AuthorId,
                        AuthorName = pa.Author.FullName,
                        AuthorContribution = pa.AuthorContribution
                    }).ToList()
                })
                .ToList();
        }

        public PublicationViewModel? GetElement(PublicationSearchModel model)
        {
            if (model == null) return null;

            using var context = new PublicationsDatabase();

            var query = context.Publications
                .Include(p => p.Source)
                .Include(p => p.PublicationAuthors)
                    .ThenInclude(pa => pa.Author)
                .AsQueryable();

            Publication? entity = null;

            if (!string.IsNullOrEmpty(model.Title))
                entity = query.FirstOrDefault(x => x.Title == model.Title);

            if (entity == null && model.Id.HasValue)
                entity = query.FirstOrDefault(x => x.Id == model.Id.Value);

            if (entity == null) return null;

            return new PublicationViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                PublicationDate = entity.PublicationDate,
                PublicationType = entity.PublicationType,
                SourceId = entity.SourceId,
                Doi = entity.Doi,
                Pages = entity.Pages,
                Abstract = entity.Abstract,
                Keywords = entity.Keywords,
                CitationCount = entity.CitationCount,
                Authors = entity.PublicationAuthors.Select(pa => new PublicationAuthorViewModel
                {
                    AuthorId = pa.AuthorId,
                    AuthorName = pa.Author.FullName,
                    AuthorContribution = pa.AuthorContribution
                }).ToList()
            };
        }


        public PublicationViewModel? Insert(PublicationBindingModel model)
        {
            if (model == null) return null;

            try
            {
                using var context = new PublicationsDatabase();
                var entity = Publication.Create(model);
                if (entity == null) return null;

                context.Publications.Add(entity);
                context.SaveChanges();

                // создаём связи с авторами и их вкладом
                if (model.Authors != null && model.Authors.Count > 0)
                {
                    foreach (var author in model.Authors)
                    {
                        context.PublicationAuthors.Add(new PublicationAuthor
                        {
                            PublicationId = entity.Id,
                            AuthorId = author.AuthorId,
                            AuthorContribution = author.AuthorContribution
                        });
                    }
                    context.SaveChanges();
                }

                // Перезагружаем entity с авторами для получения полного ViewModel
                entity = context.Publications
                    .Include(p => p.PublicationAuthors)
                        .ThenInclude(pa => pa.Author)
                    .FirstOrDefault(p => p.Id == entity.Id);

                if (entity == null) return null;

                return new PublicationViewModel
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    PublicationDate = entity.PublicationDate,
                    PublicationType = entity.PublicationType,
                    SourceId = entity.SourceId,
                    Doi = entity.Doi,
                    Pages = entity.Pages,
                    Abstract = entity.Abstract,
                    Keywords = entity.Keywords,
                    CitationCount = entity.CitationCount,
                    Authors = entity.PublicationAuthors.Select(pa => new PublicationAuthorViewModel
                    {
                        AuthorId = pa.AuthorId,
                        AuthorName = pa.Author.FullName,
                        AuthorContribution = pa.AuthorContribution
                    }).ToList()
                };
            }
            catch
            {
                return null;
            }
        }


        public PublicationViewModel? Update(PublicationBindingModel model)
        {
            if (model == null) return null;

            try
            {
                using var context = new PublicationsDatabase();
                var entity = context.Publications
                    .Include(p => p.PublicationAuthors)
                    .FirstOrDefault(x => x.Id == model.Id);
                if (entity == null) return null;

                entity.Update(model);

                // удаляем старые связи
                context.PublicationAuthors.RemoveRange(entity.PublicationAuthors);
                context.SaveChanges();

                // добавляем новые связи с вкладом авторов
                if (model.Authors != null && model.Authors.Count > 0)
                {
                    foreach (var author in model.Authors)
                    {
                        context.PublicationAuthors.Add(new PublicationAuthor
                        {
                            PublicationId = entity.Id,
                            AuthorId = author.AuthorId,
                            AuthorContribution = author.AuthorContribution
                        });
                    }
                }

                context.SaveChanges();

                // Перезагружаем с авторами
                entity = context.Publications
                    .Include(p => p.PublicationAuthors)
                        .ThenInclude(pa => pa.Author)
                    .FirstOrDefault(p => p.Id == entity.Id);

                if (entity == null) return null;

                return new PublicationViewModel
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    PublicationDate = entity.PublicationDate,
                    PublicationType = entity.PublicationType,
                    SourceId = entity.SourceId,
                    Doi = entity.Doi,
                    Pages = entity.Pages,
                    Abstract = entity.Abstract,
                    Keywords = entity.Keywords,
                    CitationCount = entity.CitationCount,
                    Authors = entity.PublicationAuthors.Select(pa => new PublicationAuthorViewModel
                    {
                        AuthorId = pa.AuthorId,
                        AuthorName = pa.Author.FullName,
                        AuthorContribution = pa.AuthorContribution
                    }).ToList()
                };
            }
            catch
            {
                return null;
            }
        }


        public PublicationViewModel? Delete(PublicationBindingModel model)
        {
            if (model == null) return null;

            try
            {
                using var context = new PublicationsDatabase();
                var entity = context.Publications
                    .Include(p => p.PublicationAuthors)
                        .ThenInclude(pa => pa.Author)
                    .FirstOrDefault(x => x.Id == model.Id);
                if (entity == null) return null;

                var viewModel = new PublicationViewModel
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    PublicationDate = entity.PublicationDate,
                    PublicationType = entity.PublicationType,
                    SourceId = entity.SourceId,
                    Doi = entity.Doi,
                    Pages = entity.Pages,
                    Abstract = entity.Abstract,
                    Keywords = entity.Keywords,
                    CitationCount = entity.CitationCount,
                    Authors = entity.PublicationAuthors.Select(pa => new PublicationAuthorViewModel
                    {
                        AuthorId = pa.AuthorId,
                        AuthorName = pa.Author.FullName,
                        AuthorContribution = pa.AuthorContribution
                    }).ToList()
                };

                context.Publications.Remove(entity);
                context.SaveChanges();
                return viewModel;
            }
            catch
            {
                return null;
            }
        }

        public List<PublicationViewModel> GetPublicationsByAuthor(int authorId)
        {
            using var context = new PublicationsDatabase();

            return context.Publications
                .Include(p => p.Source)
                .Include(p => p.PublicationAuthors)
                    .ThenInclude(pa => pa.Author)
                .Where(p => p.PublicationAuthors
                    .Any(pa => pa.AuthorId == authorId))
                .Select(p => new PublicationViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    PublicationDate = p.PublicationDate,
                    PublicationType = p.PublicationType,
                    SourceId = p.SourceId,
                    Doi = p.Doi,
                    Pages = p.Pages,
                    Abstract = p.Abstract,
                    Keywords = p.Keywords,
                    CitationCount = p.CitationCount,
                    Authors = p.PublicationAuthors.Select(pa => new PublicationAuthorViewModel
                    {
                        AuthorId = pa.AuthorId,
                        AuthorName = pa.Author.FullName,
                        AuthorContribution = pa.AuthorContribution
                    }).ToList()
                })
                .ToList();
        }

    }
}

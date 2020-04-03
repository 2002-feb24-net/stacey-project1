using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CornNuggets.DataAccess.Model;
using CornNuggets.Domain.Interfaces;

namespace CornNuggets.DataAccess.Repositories
{
    /// <summary>
    /// A repository managing data access for CornNuggets objects and their order members,
    /// using Entity Framework.
    /// </summary>
    /// <remarks>
    /// This class ought to have better exception handling and logging.
    /// </remarks>
    public class CornNuggetsRepository : ICornNuggetsRepository
    {
        private readonly CornNuggetsDbContext _dbContext;

        private readonly ILogger<CornNuggetsRepository> _logger;

        /// <summary>
        /// Initializes a new CornNuggets repository given a suitable CornNuggets data source.
        /// </summary>
        /// <param name="dbContext">The data source</param>
        /// <param name="logger">The logger</param>
        public CornNuggetsRepository(CornNuggetsDbContext dbContext, ILogger<CornNuggetsRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all CornNuggetss with deferred execution,
        /// including associated orders.
        /// </summary>
        /// <returns>The collection of CornNuggets</returns>
        public IEnumerable<Domain.Model.CornNuggets> GetCornNuggets(string search = null)
        {
            // disable unnecessary tracking for performance benefit
            IQueryable<CornNuggets> items = _dbContext.CornNuggets
                .Include(r => r.order).AsNoTracking();
            if (search != null)
            {
                items = items.Where(r => r.Name.Contains(search));
            }
            return items.Select(Mapper.MapCornNuggetsWithOrders);
        }

        /// <summary>
        /// Get a CornNuggets by ID, including any associated orders.
        /// </summary>
        /// <returns>The CornNuggets</returns>
        public Domain.Model.CornNuggets GetCornNuggetsById(int id)
        {
            CornNuggets CornNuggets = _dbContext.CornNuggets
                .Include(r => r.order)
                .FirstOrDefault(r => r.Id == id);

            return Mapper.MapCornNuggetsWithOrders(CornNuggets);
        }

        /// <summary>
        /// Add a CornNuggets, including any associated orders.
        /// </summary>
        /// <param name="CornNuggets">The CornNuggets</param>
        public void AddCornNuggets(Domain.Model.CornNuggets CornNuggets)
        {
            if (CornNuggets.Id != 0)
            {
                _logger.LogWarning("CornNuggets to be added has an ID ({CornNuggetsId}) already: ignoring.", CornNuggets.Id);
            }

            _logger.LogInformation("Adding CornNuggets");

            CornNuggets entity = Mapper.MapCornNuggetsWithOrders(CornNuggets);
            entity.Id = 0;
            _dbContext.Add(entity);
        }

        /// <summary>
        /// Delete a CornNuggets by ID. Any orders associated to it will also be deleted.
        /// </summary>
        /// <param name="CornNuggetsId">The ID of the CornNuggets</param>
        public void DeleteCornNuggets(int CornNuggetsId)
        {
            _logger.LogInformation("Deleting CornNuggets with ID {CornNuggetsId}", CornNuggetsId);
            CornNuggets entity = _dbContext.CornNuggets.Find(CornNuggetsId);
            _dbContext.Remove(entity);
        }

        /// <summary>
        /// Update a CornNuggets as well as its orders.
        /// </summary>
        /// <param name="CornNuggets">The CornNuggets with changed values</param>
        public void UpdateCornNuggets(Domain.Model.CornNuggets CornNuggets)
        {
            _logger.LogInformation("Updating CornNuggets with ID {CornNuggetsId}", CornNuggets.Id);

            // calling Update would mark every property as Modified.
            // this way will only mark the changed properties as Modified.
            CornNuggets currentEntity = _dbContext.CornNuggets.Find(CornNuggets.Id);
            CornNuggets newEntity = Mapper.MapCornNuggetsWithOrders(CornNuggets);

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Get a order.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        public Domain.Model.Order GetorderById(int orderId)
        {
            Order order = _dbContext.order.AsNoTracking()
                .First(r => r.Id == orderId);
            return Mapper.Map(order);
        }

        /// <summary>
        /// Add a order and associate it with a CornNuggets store.
        /// </summary>
        /// <param name="order">The order</param>
        /// <param name="CornNuggets">The CornNuggets store</param>
        public void Addorder(Domain.Model.Order order, Domain.Model.CornNuggets CornNuggets = null)
        {
            if (CornNuggets.Id != 0)
            {
                _logger.LogWarning("Order to be added has an ID ({orderId}) already: ignoring.", order.Id);
            }

            _logger.LogInformation("Adding order to CornNuggets with ID {CornNuggetsId}", CornNuggets.Id);

            if (CornNuggets != null)
            {
                // get the db's version of that CornNuggets
                // (can't use Find with Include)
                CornNuggets CornNuggetsEntity = _dbContext.CornNuggets
                    .Include(r => r.order)
                    .First(r => r.Id == CornNuggets.Id);
                Order newEntity = Mapper.Map(order);
                CornNuggetsEntity.order.Add(newEntity);
                // also, modify the parameters
                CornNuggets.Orders.Add(order);
            }
            else
            {
                Order newEntity = Mapper.Map(order);
                _dbContext.Add(newEntity);
            }
        }

        /// <summary>
        /// Delete a order by ID.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        public void Deleteorder(int orderId)
        {
            _logger.LogInformation("Deleting order with ID {orderId}", orderId);

            Order entity = _dbContext.Order.Find(orderId);
            _dbContext.Remove(entity);
        }

        /// <summary>
        /// Update a order.
        /// </summary>
        /// <param name="order">The order with changed values</param>
        public void Updateorder(Domain.Model.Order order)
        {
            _logger.LogInformation("Updating order with ID {orderId}", order.Id);

            Order currentEntity = _dbContext.order.Find(order.Id);
            Order newEntity = Mapper.Map(order);

            _dbContext.Entry(currentEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Get the ID of the CornNuggets associated to the order with the given ID.
        /// </summary>
        /// <param name="orderId">The ID of the order</param>
        /// <returns>The ID of the CornNuggets store</returns>
        public int CornNuggetsIdFromOrderId(int orderId)
        {
            return _dbContext.order.AsNoTracking()
                .Where(r => r.Id == orderId)
                .Select(r => r.CornNuggetsId)
                .First();
        }

        /// <summary>
        /// Persist changes to the data source.
        /// </summary>
        public void Save()
        {
            _logger.LogInformation("Saving changes to the database");
            _dbContext.SaveChanges();
        }
    }
}

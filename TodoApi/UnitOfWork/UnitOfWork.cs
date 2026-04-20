using TodoApi.Models;
using TodoApi.Repositories.TodoItems;
using TodoApi.Repositories.TodoLists;

namespace TodoApi.UOF
{
        public class UnitOfWork
        {
            private readonly CommandDbContext _context;
            private bool _disposed = false;
            public IWritableTodoItemRepository TodoItemRepo;
            public IWritableTodoListRepository TodoListRepo;
            public UnitOfWork(CommandDbContext context, ILogger<WritableTodoListRepository> _logger)
            {
                _context = context;
                TodoItemRepo = new WritableTodoItemRepository(_context);
                TodoListRepo = new WritableTodoListRepository(_context, _logger);
            }

            public async Task<int> CompleteAsync()
            {
                return await _context.SaveChangesAsync();
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _context.Dispose();
                    }
                }
                _disposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

        }
}

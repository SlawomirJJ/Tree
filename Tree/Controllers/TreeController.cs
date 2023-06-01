using Microsoft.AspNetCore.Mvc;
using Tree.Entities;
using Tree.Models;

namespace Tree.Controllers
{
    [Route("api/tree")]
    public class TreeController: ControllerBase
    {
        private readonly TreeDbContext _dbContext;
        public TreeController(TreeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // rozwiń całą strukturę
        [HttpGet]
        public ActionResult <IEnumerable<Register>> GetAll()
        {
            var registers = _dbContext
                .Registers
                .ToList();

            MappingToFolderStructure mappingToFolderStructure = new MappingToFolderStructure();
            var folderStructure = mappingToFolderStructure.GetFolderStructure (registers);

            return Ok(folderStructure);
        }
        // rozwiń jeden poziom od folderu na który kliknąłeś



        

    }
}

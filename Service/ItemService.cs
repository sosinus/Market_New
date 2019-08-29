using Models.RepositoryResults;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnitsOfWork;

namespace Service
{

    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Item> GetAllItems()
        {
            var items = _unitOfWork.ItemRepository.All().ToList<Item>();
            foreach (Item item in items)
            {
                string folderName = Path.Combine("Resources", "Images", item.Id.ToString());
                if (Directory.Exists(folderName))
                    item.Image = Directory.GetFiles(folderName)[0];
            }
            return items;
        }

        public ItemResult AddNewItem(Item item, string userId)
        {
            var result = new ItemResult();
            _unitOfWork.ItemRepository.Add(item);
            _unitOfWork.Commit();
            moveImages(item.Id.ToString(), userId);
            result.Success = true;
            return result;
        }

        public ItemResult DeleteItem(int id)
        {
            var result = new ItemResult();
            var item = _unitOfWork.ItemRepository.All().Single(i => i.Id == id);
            _unitOfWork.ItemRepository.Delete(item);
            _unitOfWork.Commit();
            result.Success = true;
            return result;
        }

        public ItemResult UpdateItem(Item item, string userId)
        {
            var result = new ItemResult();
            _unitOfWork.ItemRepository.Update(item);
            _unitOfWork.Commit();
            moveImages(item.Id.ToString(), userId);
            result.Success = true;
            return result;
        }

        public void moveImages(string id, string userId)
        {           
            string srcDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", "Temp", userId);
            string dstDir = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", id);
            if (Directory.Exists(dstDir))
            {
                foreach (var file in Directory.GetFiles(dstDir))
                {
                    System.IO.File.Delete(file);
                }
            }

            else
                Directory.CreateDirectory(dstDir);
            string[] files = Directory.GetFiles(srcDir);
            foreach (string file in files)
            {
                var fileName = Path.GetFileName(file);
                string dstFile = Path.Combine(dstDir, fileName);
                if (!System.IO.File.Exists(dstFile))
                    System.IO.File.Move(file, dstFile);
            }
            Directory.Delete(srcDir, true);
        }
    }
}

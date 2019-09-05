using Models;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		public OperationResult AddNewItem(Item item, string userId)
		{
			try
			{
				_unitOfWork.ItemRepository.Add(item);
				_unitOfWork.Commit();
				moveImages(item.Id.ToString(), userId);
				return new OperationResult { Succeeded = true };
			}
			catch (Exception ex)
			{
				return new OperationResult { Succeeded = false, Message = ex.Message };
			}

		}

		public OperationResult DeleteItem(int id)
		{
			try
			{
				var item = _unitOfWork.ItemRepository.All().Single(i => i.Id == id);
				_unitOfWork.ItemRepository.Delete(item);
				_unitOfWork.Commit();
                string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Images", id.ToString());
                if (Directory.Exists(imgPath))
                    Directory.Delete(imgPath, true);
                return new OperationResult { Succeeded = true };
			}
			catch (Exception ex)
			{
				return new OperationResult { Succeeded = false, Message = ex.Message };
			}


		}

		public OperationResult UpdateItem(Item item, string userId)
		{
			try
			{
				_unitOfWork.ItemRepository.Update(item);
				_unitOfWork.Commit();
				moveImages(item.Id.ToString(), userId);
				return new OperationResult { Succeeded = true };
			}
			catch (Exception ex)
			{
				return new OperationResult { Succeeded = false, Message = ex.Message };
			}
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

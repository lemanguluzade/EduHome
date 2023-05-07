﻿using EduHomee.DAL;
using EduHomee.Helper;
using EduHomee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace EduHomee.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SlidersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public SlidersController(AppDbContext db,IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _db.Sliders.ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            #region SAVE IMAGE
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image cannot be null");
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image type");
                return View();
            }
            if (slider.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "MAX 1 Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "img", "slider");
            slider.Image = await slider.Photo.SaveFileAsync(folder);
            #endregion

            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            if (dbSlider.IsDeactive)
            {
                dbSlider.IsDeactive = false;
            }
            else
            {
                dbSlider.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            return View(dbSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id,Slider slider)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            #region SAVE IMAGE
            if (slider.Photo != null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image type");
                    return View();
                }
                if (slider.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "MAX 1 Mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                slider.Image = await slider.Photo.SaveFileAsync(folder);

                string path = Path.Combine(_env.WebRootPath, folder, dbSlider.Image);
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                dbSlider.Image = slider.Image;
            }
           
            #endregion

            dbSlider.Title=slider.Title;
            dbSlider.Description=slider.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }

            return View(dbSlider);
        }


    }
}

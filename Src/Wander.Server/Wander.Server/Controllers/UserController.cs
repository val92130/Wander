﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wander.Server.Models;

namespace Wander.Server.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public ActionResult SignUp(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                if (user.CheckRegisterForm())
                {
                   user.Register();
                   return RedirectToAction("Index", "Home");
                }

            }
            return RedirectToAction("Index", "Home", new {error = "Error, cannot sign you up"});
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                if (user.CheckLogin())
                {
                    user.Connect();
                    Session["Login"] = user.Login;
                    Debug.Print(Session["Login"].ToString());
                    return RedirectToAction("Index", "Home");
                }
               
            }
            return RedirectToAction("Index", "Home", new { error = "Error, wrong login / password" });
        }
        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
           
        }

    }
}
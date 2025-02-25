﻿using CASServiceLayer.Models;
using DALLayer;
using System;
using System.Web.Mvc;

namespace CASUILayer.Controllers
{
    public class HomeController : Controller
    {
        private  ServiceOperations service; 

        public HomeController()
        {
            service = new ServiceOperations();
        }

        public ActionResult Index()//landing page
        {
            return View();          
        }
  /*---------------------------------------------------------------------------------Admin--------------------------------------------------------------------*/

        public ActionResult AdminLogin()  
        {                                  
            Session["SId"] = 1;            
            return View();               
        }

        [HttpPost]
        public ActionResult AdminLogin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                if (service.checkAdminLogin(admin)) //checks the admin details are vaild or not
                {
                    Admin ad = service.FindAdminByEmail(admin.EmailId);  
                    Session["AdminObject"] = ad; 

                    return RedirectToAction("AdminIndex", "Admins");
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();   
                }
            }
            return View();
        }
/*---------------------------------------------------------------------------------Doctor--------------------------------------------------------------------*/
        public ActionResult DoctorLogin()
        {
            Session["SId"] = 2;
            return View();
        }

        [HttpPost]
        public ActionResult DoctorLogin(string Email,string Password)
        {
            try
            {
                if (Email != null && Password != null)
                {
                    if (service.checkDoctorLogin(Email,Password))
                    {
                        Doctor doc = service.FindDoctorByEmail(Email);
                        Session["DocObject"] = doc;
                        return RedirectToAction("AppointmentList", "Doctors");
                    }
                    else
                    {
                        ModelState.AddModelError("", "InCorrect Email ID and Password");
                        return View();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
         }
  /*------------------------------------------------------------------------------Patient--------------------------------------------------------------------*/

        public ActionResult PatientLogin()
        {
            Session["SId"] = 3;
            return View();
        }

        [HttpPost]
        public ActionResult PatientLogin(Patient patient)
        {
            if (patient.Email != null && patient.Password != null)
            {
                if (service.checkPatientLogin(patient))
                 {
                    Patient patient1 = service.FindPatientByEmail(patient.Email);
                    Session["PatientObj"] = patient1;
                    return RedirectToAction("Index", "Patients");
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();
                }
            }
            return View();
        }

        public ActionResult PatientSignUp() 
        { 
            return View(); 
        }


        [HttpPost]
        public ActionResult PatientSignUp(Patient patient)
        {
           
            if (DateTime.Now < patient.DOB)
            {
                ModelState.AddModelError("DOB", "Please select a valid date of birth.");
                return View();
            }

            if (ModelState.IsValid)
            {
               service.InsertPatient(patient);
               return RedirectToAction("PatientLogin");
            }
            return View();
        }
 /*----------------------------------------------------------------------------Front Office--------------------------------------------------------------------*/

        public ActionResult FrontOfficeLogin()
        {
            Session["SId"] = 4;
        
            return View();
        }

        [HttpPost]
        public ActionResult FrontOfficeLogin(FrontOfficeExecutive frontOffice)
        {
            if (frontOffice.Email != null && frontOffice.Password != null)
            {
                if (service.checkFOLogin(frontOffice))
                {
                    FrontOfficeExecutive FE1 = service.FindFrontOfficeByEmail(frontOffice.Email);
                    Session["FOObject"] = FE1;
                    return RedirectToAction("ViewAppointment", "FrontOfficeExecutives");  
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();
                }
            }
            return View();
        }
  /*-----------------------------------------------------------------------------Pharmacists--------------------------------------------------------------------*/

        public ActionResult PharmacistsLogin()
        {
            Session["SId"] = 5;
            return View();
        }

        [HttpPost]
        public ActionResult PharmacistsLogin(Pharmacist pharmacist)
        {
            if (pharmacist.Email != null && pharmacist.Password != null)
            {

                if (service.checkPharmacistLogin(pharmacist))
                {
                    Pharmacist Ph1 = service.FindPharmacistByEmail(pharmacist.Email);
                    Session["PhObject"] = Ph1;
                    return RedirectToAction("Index", "Pharmacists");
                }
                else
                {
                    ModelState.AddModelError("", "InCorrect Email ID and Password");
                    return View();
                }
            }
            return View();
        }
/*------------------------------------------------------------------------------Reset--------------------------------------------------------------------*/

        public ActionResult ResetPass()
        {
            return View();
        }
        
          
        [HttpPost]
        public ActionResult ResetPass(string email,string pass1,string pass2,int SId)
        {
            switch (SId) 
            {
                case 1:
                    if (service.Checkpass(pass1, pass2)) 
                    {
                        Admin ad = service.FindAdminByEmail(email); 
                        if (ad == null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        ad.Password = pass1;
                        service.UpdateAdmin(ad);
                        return RedirectToAction("AdminLogin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Both Password Must Be Same");
                    }
                  
                    break;
                case 2:
                    if (service.Checkpass(pass1, pass2))
                    {
                        Doctor doc = service.FindDoctorByEmail(email);
                        if(doc== null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        doc.Password = pass1;
                        service.UpdateDoctor(doc);
                        return RedirectToAction("DoctorLogin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Both Password Must Be Same");
                    }

                    break;
                case 3:
                    if (service.Checkpass(pass1, pass2))
                    {
                        
                        Patient patient = service.FindPatientByEmail(email);
                        if (patient == null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        patient.Password = pass1;
                        service.UpdatePatient(patient);
                        return RedirectToAction("PatientLogin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Both Password Must Be Same");
                    }

                    break;
                case 4:
                    if (service.Checkpass(pass1, pass2))
                    {
                        FrontOfficeExecutive Fo = service.FindFrontOfficeByEmail(email);
                        if (Fo == null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        Fo.Password = pass1;
                        service.UpdateFrontOfficeExecutive(Fo);
                        return RedirectToAction("FrontOfficeLogin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Both Password Must Be Same");
                    }

                    break;
                        
                case 5:
                    if (service.Checkpass(pass1, pass2))
                    {
                        Pharmacist ph = service.FindPharmacistByEmail(email);
                        if (ph == null)
                        {
                            ModelState.AddModelError("", "Email id not found");
                            return View();
                        }
                        ph.Password = pass1;
                        service.UpdatePharmacist(ph);
                        return RedirectToAction("PharmacistsLogin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Both Password Must Be Same");
                    }

                    break;
                default:
                    break;
            }

            return View();
        }


        public ActionResult LogOut() 
        {
            Session.Clear();
            return RedirectToAction("Index","Home");
         }
    }
}
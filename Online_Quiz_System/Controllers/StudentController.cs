﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Online_Quiz_System.Models;
using Online_Quiz_System.Common;
namespace Online_Quiz_System.Controllers
{
    public class StudentController : Controller
    {
        User user = new User();
        StudentDA Model = new StudentDA();
        // GET: Student
        public ActionResult Index(FormCollection form)
        {
            if (!user.IsStudent())
                return View("Error");
            if (user.IsTesting())
                return RedirectToAction("DoingTest");
            Model.UpdateLastLogin();
            Model.UpdateLastSeen("Trang Chủ", Url.Action("Index"));
            //int id_subject = Convert.ToInt32(form["txtSearch"]);

            string text = form["txtSearch"];      // text tìm kiếm 
            int sub_droplist = Convert.ToInt32(form["id_subject"]);   // droplist 

            if (sub_droplist == 0 && String.IsNullOrEmpty(text))
            {
                ViewBag.ListSubject = Model.GetSubjects();
                ViewBag.score = Model.GetStudentTestcode();
                return View(Model.GetDashboard());
            }
            else
            {
                if (String.IsNullOrEmpty(text) && sub_droplist > 0)
                {
                    ViewBag.ListSubject = Model.GetSubjects();
                    ViewBag.score = Model.GetStudentTestcode();
                    return View(Model.GetDashboardBySubject(sub_droplist));
                }
                else
                {
                    if (!String.IsNullOrEmpty(text) && sub_droplist == 0)
                    {
                        ViewBag.ListSubject = Model.GetSubjects();
                        ViewBag.score = Model.GetStudentTestcode();
                        return View(Model.GetDashboardByName(text));
                    }

                    ViewBag.ListSubject = Model.GetSubjects();
                    ViewBag.score = Model.GetStudentTestcode();
                    return View(Model.GetDashboardSubject_Name(sub_droplist, text));
                }
            }
        }

        public ActionResult DeLT_Student(FormCollection form)
        {
            if (!user.IsStudent())
                return View("Error");
            if (user.IsTesting())
                return RedirectToAction("DoingTest");
            Model.UpdateLastLogin();
            Model.UpdateLastSeen("Trang Chủ - đề LT", Url.Action("DeLT_Student"));
            //int id_subject = Convert.ToInt32(form["txtSearch"]);

            string text = form["txtSearch"];      // text tìm kiếm 
            int sub_droplist = Convert.ToInt32(form["id_subject"]);   // droplist 

            if (sub_droplist == 0 && String.IsNullOrEmpty(text))
            {
                ViewBag.ListSubject = Model.GetSubjects();
                ViewBag.score = Model.GetStudentTestcode();
                return View(Model.GetDeLT());
            }
            else
            {
                if (String.IsNullOrEmpty(text) && sub_droplist > 0)
                {
                    ViewBag.ListSubject = Model.GetSubjects();
                    ViewBag.score = Model.GetStudentTestcode();
                    return View(Model.GetDeLTBySubject(sub_droplist));
                }
                else
                {
                    if (!String.IsNullOrEmpty(text) && sub_droplist == 0)
                    {
                        ViewBag.ListSubject = Model.GetSubjects();
                        ViewBag.score = Model.GetStudentTestcode();
                        return View(Model.GetDeLTByName(text));
                    }

                    ViewBag.ListSubject = Model.GetSubjects();
                    ViewBag.score = Model.GetStudentTestcode();
                    return View(Model.GetDeLTSubject_Name(sub_droplist, text));
                }
            }
        }

        [HttpPost]
        public ActionResult CheckPassword(FormCollection form)
        {
            if (!user.IsStudent())
                return View("Error");
            if (user.IsTesting())
                return RedirectToAction("DoingTest");
            string test_code = form["test_code"];
            int code = Convert.ToInt32(test_code);

            // mã hoá mật khẩu đề thi nhập vào 
            //string password = Encryptor.MD5Hash(form["password"]);
            string password = Encryptor.EncodePassword(form["password"]);

            string test_password = Model.GetTest(code).password;
            if (!password.Equals(test_password))
            {
                TempData["status_id"] = false;
                TempData["status"] = "Mật khẩu không đúng!";
                return RedirectToAction("Index");
            } else
            {
                Model.CreateStudentQuestion(code);
                Model.UpdateStatus(code, Model.GetTest(code).time_to_do + ":00");
                return RedirectToAction("DoingTest");
            }
        }
        public ActionResult CheckDeLT(FormCollection form)
        {
            if (!user.IsStudent())
                return View("Error");
            if (user.IsTesting())
                return RedirectToAction("DoingDeLT");

            string test_code = form["test_code"];
            int code = Convert.ToInt32(test_code);

            Model.CreateStudentQuestionDELT(code);
            Model.UpdateStatus(code, Model.GetTest(code).time_to_do + ":00");
            return RedirectToAction("DoingDeLT");            
        }

        public ActionResult DoingTest()
        {
            if (!user.IsStudent())
                return View("Error");
            if (!user.IsTesting())
                return View("Error");
            if(user.TIME != null)
            {
                string[] time = Regex.Split(user.TIME, ":");
                ViewBag.min = time[0];
                ViewBag.sec = time[1];
            }
            return View(Model.GetListQuest(user.TESTCODE));
        }

        public ActionResult DoingDeLT()
        {
            if (!user.IsStudent())
                return View("Error");
            if (!user.IsTesting())
                return View("Error");
            if (user.TIME != null)
            {
                string[] time = Regex.Split(user.TIME, ":");
                ViewBag.min = time[0];
                ViewBag.sec = time[1];
            }         
            return View(Model.GetListQuest(user.TESTCODE));
        }

        public ActionResult SubmitTest()
        {
            if (!user.IsStudent())
                return View("Error");

            if (!user.IsTesting())
                return View("Error");

            // Lấy danh sách câu hỏi
            var list = Model.GetListQuest(user.TESTCODE);

            // Kiểm tra danh sách có dữ liệu
            if (list == null || !list.Any())
            {
                return View("Error", (object)"Danh sách câu hỏi trống hoặc không tồn tại.");
            }

            // Lấy thông tin bài kiểm tra
            int total_quest = list.First().test.total_questions;
            int test_code = list.First().test.test_code;

            // Tính toán điểm
            double coefficient = 10.0 / (double)total_quest;
            int count_correct = 0;

            foreach (var item in list)
            {
                if (item.student_test.student_answer != null
                    && item.student_test.student_answer.Trim().Equals(item.question.correct_answer.Trim()))
                {
                    count_correct++;
                }
            }

            double score = coefficient * count_correct;
            string detail = count_correct + "/" + total_quest;

            // Lưu điểm vào cơ sở dữ liệu
            Model.InsertScore(score, detail);

            // Hoàn tất bài kiểm tra
            Model.FinishTest();

            return RedirectToAction("NhanKQTest/" + test_code);
        }


        public ActionResult SubmitDeLT()
        {
            if (!user.IsStudent())
                return View("Error");
            if (!user.IsTesting())
                return View("Error");
            var list = Model.GetListQuest(user.TESTCODE);
            int total_quest = list.First().test.total_questions; 
            int test_code = list.First().test.test_code;
            double coefficient = 10.0 / (double)total_quest;
            int count_correct = 0;
            foreach (var item in list)
            {
                if (item.student_test.student_answer != null && item.student_test.student_answer.Trim().Equals(item.question.correct_answer.Trim()))
                    count_correct++;
            }
            double score = coefficient * count_correct;
            string detail = count_correct + "/" + total_quest;
            Model.InsertScoreDELT(score, detail);
            Model.FinishTest();
            return RedirectToAction("PreviewDeLT/" + test_code);
        }

        public ActionResult NhanKQTest(int id)
        {
            if (!user.IsStudent())
                return View("Error");
            if (user.IsTesting())
                return RedirectToAction("DoingTest");
            if (Model.GetStudentTestcode().IndexOf(id) == -1)
                return View("Error");
            ViewBag.score = Model.GetScore(id);
            return View(Model.GetListQuest(id));
        }

        public ActionResult PreviewTest(int id)
        {
            if (!user.IsStudent())
                return View("Error");
            if (user.IsTesting())
                return RedirectToAction("DoingTest");
            if(Model.GetStudentTestcode().IndexOf(id) == -1)
                return View("Error");
            ViewBag.score = Model.GetScore(id);
            return View(Model.GetListQuest(id));
        }

        public ActionResult PreviewDeLT(int id)
        {
            if (!user.IsStudent())
                return View("Error");
            if (user.IsTesting())
                return RedirectToAction("DoingDeLT");
            if (Model.GetStudentTestcode().IndexOf(id) == -1)
                return View("Error");
            ViewBag.score = Model.GetScore(id);
            return View(Model.GetListQuest(id));
        }


        [HttpPost]
        public void UpdateTiming(FormCollection form)
        {
            string min = form["min"];
            string sec = form["sec"];
            string time = min + ":" + sec;
            Model.UpdateTiming(time);
        }
        [HttpPost]
        public void UpdateStudentTest(FormCollection form)
        {
            int id_quest = Convert.ToInt32(form["id"]);
            string answer = form["answer"];
            answer = answer.Trim();
            string time = form["min"] + ":" + form["sec"];
            Model.UpdateStudentTest(id_quest, answer);
            Model.UpdateTiming(time);
        }

        public ActionResult ThongKeHT()
        {
            if (!user.IsStudent())
                return View("Error");
            // đề thi
            var list_sum = Model.GetScore_ByID(user.ID);
            ViewBag.sum_test = list_sum.Count;
            var list_pass = Model.GetScorePass_ByID(user.ID);
            ViewBag.pass_test = list_pass.Count;
            var list_fail = Model.GetScoreFail_ByID(user.ID);
            ViewBag.fail_test = list_fail.Count;

            //đề Luyện tập 

            var list_sum_LT = Model.GetScoreLT_ByID(user.ID);
            ViewBag.sum_testLT = list_sum_LT.Count;
            var list_pass_LT = Model.GetScorePassLT_ByID(user.ID);
            ViewBag.pass_testLT = list_pass_LT.Count;
            var list_fail_LT = Model.GetScoreFailLT_ByID(user.ID);
            ViewBag.fail_testLT = list_fail_LT.Count;


            return View(list_sum);
        }

        public ActionResult Logout()
        {
            if (!user.IsStudent())
                return View("Error");
            user.Reset();
            return RedirectToAction("Index", "Login");
        }
    }
}
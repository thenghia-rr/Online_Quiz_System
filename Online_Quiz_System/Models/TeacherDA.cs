﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Online_Quiz_System.Common;
namespace Online_Quiz_System.Models
{
    public class TeacherDA
    {
        User user = new User();
        trac_nghiem_onlineEntities db = new trac_nghiem_onlineEntities();

        public void UpdateLastLogin()
        {
            var update = (from x in db.teachers where x.id_teacher == user.ID select x).Single();
            update.last_login = DateTime.Now;
            db.SaveChanges();
        }
        public void UpdateLastSeen(string name, string url)
        {
            var update = (from x in db.teachers where x.id_teacher == user.ID select x).Single();
            update.last_seen = name;
            update.last_seen_url = url;
            db.SaveChanges();
        }

        public List<grade> GetGrades()
        {
            return db.grades.ToList();
        }

        public @class GetClass(int id)
        {
            @class cl = new @class();
            try
            {
                cl = db.classes.SingleOrDefault(x => x.id_class == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return cl;
        }
        public List<subject> GetSubjects()
        {
            return db.subjects.ToList();
        }

        public List<TestViewModel> GetListTest()
        {
            List<TestViewModel> tests = (from x in db.tests
                                         join s in db.subjects on x.id_subject equals s.id_subject
                                         join stt in db.statuses on x.id_status equals stt.id_status
                                         where x.type == 1
                                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            return tests;
        }
        public List<TestViewModel> DeLuyenTap()
        {
            List<TestViewModel> tests = (from x in db.tests
                                         join s in db.subjects on x.id_subject equals s.id_subject
                                         join stt in db.statuses on x.id_status equals stt.id_status
                                         where x.type == 2
                                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            return tests;
        }

        public List<TestViewModel> GetListTestBySubject(int id_subject1)
        {
            List<TestViewModel> tests = new List<TestViewModel>();
            try
            {
                tests = (from x in db.tests
                         join s in db.subjects on x.id_subject equals s.id_subject
                         join stt in db.statuses on x.id_status equals stt.id_status
                         where s.id_subject == id_subject1 && x.type == 1
                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
            }
            return tests;
        }

        public List<TestViewModel> DeLuyenTapBySubject(int id_subject1)
        {
            List<TestViewModel> tests = new List<TestViewModel>();
            try
            {
                tests = (from x in db.tests
                         join s in db.subjects on x.id_subject equals s.id_subject
                         join stt in db.statuses on x.id_status equals stt.id_status
                         where s.id_subject == id_subject1 && x.type == 2
                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
            }
            return tests;
        }

        public List<TestViewModel> GetListTestBySubject_Name(int id_subject1, string name_test)
        {
            if (!String.IsNullOrEmpty(name_test)) { name_test = name_test.ToLower().Trim(); }
            List<TestViewModel> tests = new List<TestViewModel>();
            try
            {
                tests = (from x in db.tests
                         join s in db.subjects on x.id_subject equals s.id_subject
                         join stt in db.statuses on x.id_status equals stt.id_status
                         where (s.id_subject == id_subject1) && (x.test_name.ToLower().Contains(name_test)) && (x.type == 1)
                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
            }
            return tests;
        }

        public List<TestViewModel> DeLuyenTapSubject_Name(int id_subject1, string name_test)
        {
            if (!String.IsNullOrEmpty(name_test)) { name_test = name_test.ToLower().Trim(); }
            List<TestViewModel> tests = new List<TestViewModel>();
            try
            {
                tests = (from x in db.tests
                         join s in db.subjects on x.id_subject equals s.id_subject
                         join stt in db.statuses on x.id_status equals stt.id_status
                         where (s.id_subject == id_subject1) && (x.test_name.ToLower().Contains(name_test)) && (x.type == 2)
                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
            }
            return tests;
        }

        public List<TestViewModel> GetListTestByName(string name_test)
        {
            if (!String.IsNullOrEmpty(name_test)) { name_test = name_test.ToLower().Trim(); }

            List<TestViewModel> tests = new List<TestViewModel>();
            try
            {
                tests = (from x in db.tests
                         join s in db.subjects on x.id_subject equals s.id_subject
                         join stt in db.statuses on x.id_status equals stt.id_status
                         where x.test_name.ToLower().Contains(name_test) && x.type == 1
                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
            }
            return tests;
        }

        public List<TestViewModel> DeLuyenTapByName(string name_test)
        {
            if (!String.IsNullOrEmpty(name_test)) { name_test = name_test.ToLower().Trim(); }

            List<TestViewModel> tests = new List<TestViewModel>();
            try
            {
                tests = (from x in db.tests
                         join s in db.subjects on x.id_subject equals s.id_subject
                         join stt in db.statuses on x.id_status equals stt.id_status
                         where x.test_name.ToLower().Contains(name_test) && x.type == 2
                         select new TestViewModel { test = x, subject = s, status = stt }).ToList();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1);
            }
            return tests;
        }

        public test GetTest(int test_code)
        {
            test test = new test();
            try
            {
                test = db.tests.SingleOrDefault(x => x.test_code == test_code);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return test;
        }
        
        public List<ScoreViewModel> GetListScore(int test_code)
        {
            List<ScoreViewModel> score = new List<ScoreViewModel>();
            try
            {
                score = (from x in db.scores
                         join s in db.students on x.id_student equals s.id_student
                         where x.test_code == test_code
                         select new ScoreViewModel { score = x, student = s }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return score;
        }
    }
}
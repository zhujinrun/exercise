using BlazorApp.Shared;
using BlazorWebApi.Server.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebApi.Server.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private IUserInfoService _userInfoService;
        public UserController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        [HttpGet]
        [Route("GetByDeptId/{deptId}")]
        public IList<UserInfo> GetAllForDepartment(int deptId)
        {
            return _userInfoService.GetForDepartment(new UserParameters()).Where(x=>x.DeptId==deptId).ToList();
        }
        [HttpGet]
        [Route("GetUser/{userId}")]
        public UserInfo GetUser(int userId)
        {
            return _userInfoService.GetForDepartment(new UserParameters()).Where(m => m.UserID == userId).SingleOrDefault();
        }
        [HttpPost]
        [Route("AddUser")]
        public UserInfo AddUser(UserInfo userInfo)
        {
            return _userInfoService.Add(userInfo);
        }

        [HttpGet]
        [Route("GetAll")]
        public List<UserInfo> GetAll()
        {
            return _userInfoService.GetForDepartment(new UserParameters());
        }
    }
}

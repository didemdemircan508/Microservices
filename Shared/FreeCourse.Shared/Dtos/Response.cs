﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; private set; }

        [JsonIgnore]//when ı request the API ,we can see responce type :tı given example:200,404 but we need this
        public int StatusCode { get; set; }


        [JsonIgnore]
        public bool IsSuccessful { get; set; }

        public List<string> Errors { get; set; }
        //static factory method
        public static Response<T> Success(T data, int statusCode)//we dont need to new instance when we want to use it
        { 
          return new Response<T> { Data = data, StatusCode = statusCode,IsSuccessful=true };
        }

        public static Response<T> Success(int statusCode)//we dont need to new instance when we want to use it
        {
            return new Response<T> {Data=default(T),  StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        { 
          return new Response<T> { Errors = errors, StatusCode = statusCode,IsSuccessful = false};
        }

        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode,IsSuccessful=false };
        }





    }
}

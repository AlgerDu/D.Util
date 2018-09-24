using System;
using System.Collections.Generic;
using System.Text;

namespace D.Utils
{
    /// <summary>
    /// 默认的一些返回值结果
    /// </summary>
    public enum DefaultReultCode
    {
        /// <summary>
        /// 默认成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 默认失败
        /// </summary>
        Error = 1
    }

    /// <summary>
    /// IResult 实现
    /// </summary>
    public class Result : IResult
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 创建一个 Result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result Create(int code, string msg = null)
        {
            return new Result
            {
                Code = code,
                Msg = msg
            };
        }

        static Result Create(DefaultReultCode code, string msg = null)
        {
            return Create((int)code, msg);
        }

        /// <summary>
        /// 创建一个 Result T
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> Create<T>(int code, T data = default(T), string msg = null)
        {
            return new Result<T>
            {
                Code = code,
                Data = data,
                Msg = msg
            };
        }

        /// <summary>
        /// 创建一个 error Result
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result CreateError(string msg = null)
        {
            return Create(DefaultReultCode.Error, msg);
        }

        /// <summary>
        /// 创建一个 Result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result CreateError(int code, string msg = null)
        {
            return Create(code, msg);
        }

        /// <summary>
        /// 创建一个 Result
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result CreateSuccess(string msg = null)
        {
            return Create(DefaultReultCode.Success, msg);
        }

        /// <summary>
        /// 创建一个 Result
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result CreateSuccess(int code, string msg = null)
        {
            return Create(code, msg);
        }

        /// <summary>
        /// 创建一个 Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> CreateError<T>(string msg = null)
        {
            return Create((int)DefaultReultCode.Error, default(T), msg);
        }

        /// <summary>
        /// 创建一个 Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> CreateError<T>(T data, string msg = null)
        {
            return Create((int)DefaultReultCode.Error, data, msg);
        }

        /// <summary>
        /// 创建一个 Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> CreateSuccess<T>(T data, string msg = null)
        {
            return Create((int)DefaultReultCode.Success, data, msg);
        }

        /// <summary>
        /// 创建一个 Result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result<T> CreateSuccess<T>(int code, T data, string msg = null)
        {
            return Create(code, data, msg);
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess()
        {
            return Code % 10 == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({Code})[Msg={Msg}]";
        }
    }

    /// <summary>
    /// IResult 实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result, IResult<T>
    {
        /// <summary>
        /// 泛型数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({Code})[Msg={Msg},Data={(Data == null ? "null" : "not null")}]";
        }
    }
}

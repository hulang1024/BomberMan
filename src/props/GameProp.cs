using Godot;
using System;

namespace GameProps
{
    public enum PropType
    {
        /// <summary>
        /// 火力
        /// </summary>
        Power,
        /// <summary>
        /// 弹药
        /// </summary>
        Bomb,
        /// <summary>
        /// 控制器
        /// </summary>
        Controller,
        /// <summary>
        /// 溜冰鞋
        /// </summary>
        Skates,
        /// <summary>
        /// 神行
        /// </summary>
        Freelander,
        /// <summary>
        /// 土遁
        /// </summary>
        Toton,
        /// <summary>
        /// 无敌
        /// </summary>
        Invincible,
        /// <summary>
        /// 火遁
        /// </summary>
        Katon,
        /// <summary>
        /// 生命
        /// </summary>
        Life
    }

    public abstract class GameProp
    {
        public PropType Type { get; internal set; }

        public abstract void Use(Character character);
    }
}
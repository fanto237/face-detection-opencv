import React, { useState } from "react";
import logo from "../assets/logo.png";
import { Link } from "react-router-dom";
import { AiOutlineMenu, AiOutlineClose } from "react-icons/ai";

function Navbar() {
  const [isMobil, SetIsMobil] = useState(false);

  const handleNav = () => {
    SetIsMobil(!isMobil);
    console.log(isMobil);
  };

  return (
    <div className="flex items-center justify-between h-40 max-w-[1240px] px-4 mx-auto">
      <Link to="/" className="h-full">
        <img
          src={logo}
          alt="this is the logo of the site"
          className="h-full"
        ></img>
      </Link>

      <ul className="hidden md:flex">
        <li className="p-4 text-black bg-[#1bd4f1]  rounded">
          <Link to="/upload" className="text-black bg-[#1bd4f1]">
            Get Started
          </Link>
        </li>
        <li className="p-4 text-[#1bd4f1]">
          {" "}
          <Link to="/status">Status</Link>
        </li>
        <li className="p-4 text-[#1bd4f1]">
          {" "}
          <Link to="/terms-of-usage">Terms of usage</Link>
        </li>
        <li className="p-4 text-[#1bd4f1]">
          {" "}
          <Link to="/about">About</Link>
        </li>
      </ul>

      <div onClick={handleNav} className="block md:hidden">
        {isMobil ? (
          <AiOutlineClose size={20}></AiOutlineClose>
        ) : (
          <AiOutlineMenu size={20}></AiOutlineMenu>
        )}
      </div>

      <div
        className={
          isMobil
            ? "fixed top-0 left-0 w-3/5 h-full border-r border-r-gray-900 ease-in-out duration-500 md:left-[-100%]"
            : "fixed left-[-100%]"
        }
      >
        <Link to="/" className="h-full">
          <img
            src={logo}
            alt="this is the logo of the site"
            className="h-40 mx-auto w-50"
          ></img>
        </Link>
        <ul className="p-4 pt-12 text-center uppercase">
          <li className="p-4 text-black bg-[#1bd4f1] rounded">
            <Link
              to="/upload"
              className="text-black bg-[#1bd4f1]"
              onClick={handleNav}
            >
              Get Started
            </Link>
          </li>
          <li className="p-4 text-[#1bd4f1]">
            <Link to="/status" onClick={handleNav}>
              Status
            </Link>
          </li>
          <li className="p-4 text-[#1bd4f1]">
            <Link to="/terms-of-usage" onClick={handleNav}>
              Terms of usage
            </Link>
          </li>
          <li className="p-4 text-[#1bd4f1]">
            <Link to="/about" onClick={handleNav}>
              About
            </Link>
          </li>
        </ul>
      </div>
    </div>
  );
}

export default Navbar;

import React from "react";
import { BiSearch } from "react-icons/bi";

function Status() {
  return (
    <div className="w-full mx-auto">
      <h1 className="text-[#1bd4f1] text-center text-3xl md:text-5xl pt-10">
        Track an order
      </h1>

      <div className="flex items-center justify-center w-full py-10">
        <input type="text" placeholder="Enter your order number" className=" border-[white] border-2 rounded w-[30%] text-center py-1" />
        <button className="mx-10 px-16 py-1 rounded bg-[#1bd4f1] flex  items-center"><BiSearch size={28} className="bg-[#1bd4f1]" /><span className="pl-1 text-xl uppercase bg-inherit">Track</span></button>
      </div>

    </div>)
}

export default Status;

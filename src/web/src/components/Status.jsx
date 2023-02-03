import React from "react";
import { BiSearch } from "react-icons/bi";

function Status() {
  return (
    <div className="flex flex-col items-center justify-center w-full">
      <h1 className="text-[#1bd4f1] text-center text-3xl md:text-5xl pt-10">
        Track your order
      </h1>

      <div className="flex flex-col items-center justify-center w-full py-10 sm:flex-row ">
        <input type="text" placeholder="Enter your order number" className=" border-[white] border-2 rounded  text-center py-1 w-[320px] sm:w-96" />
        <button className=" mt-2 sm:mt-0 sm:ml-2 py-[2px] px-2 rounded bg-[#1bd4f1] flex  items-center"><BiSearch size={28} className="bg-[#1bd4f1]" /><span className="pl-1 uppercase text-2sm bg-inherit">Track</span></button>
      </div>

    </div>
  )
}

export default Status;

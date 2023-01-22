import React from "react";
import Typed from "react-typed";
import { Link } from "react-router-dom";

function Home() {
  return (
    <div className="flex items-center justify-center h-[75vh]">
      <div className="max-w-[800px] m-auto  text-center flex flex-col">
        <p className="uppercase text-[#1bd4f1] font-bold p-2">
          Detect any face from any image
        </p>
        <p className="text-4xl font-bold md:text-7xl sm:text-6xl md:py-6">
          Face Recognition for Everyone
        </p>
        <div className="flex items-center justify-center p-2">
          <p className="text-xl font-bold md:text-5xl sm:text-4xl">
            It is really{" "}
          </p>
          <Typed
            className="pl-2 text-xl font-bold text-[#1bd4f1] underline md:text-5xl sm:text-4xl"
            strings={["EASY", "FLEXIBEL", "RELIABLE"]}
            typeSpeed={100}
            backSpeed={110}
            loop
            style={{
              color: "#1bd4f1",
            }}
          ></Typed>
        </div>
        <button
          about="#"
          className="bg-[#1bd4f1] text-black w-40 mx-auto mt-6 rounded p-2"
        >
          {" "}
          <Link to="/upload" className="text-black bg-[#1bd4f1]">
            Get Started
          </Link>
        </button>
      </div>
    </div>
  );
}

export default Home;

import React from "react";
// import Typed from "react-typed";

function Home() {
  return (
    <div>
      <div className="max-w-[800px] mt-40 md:mt-60 sm:mt-40 w-full h-[67vh] min-h-fit mx-auto text-center flex flex-col">
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
          {/* <Typed
            className="pl-2 text-xl font-bold text-[#1bd4f1] underline md:text-5xl sm:text-4xl"
            strings={["EASY", "FLEXIBEL"]}
            typeSpeed={100}
            backSpeed={110}
            loop
            style={{
              color: "#1bd4f1",
            }}
          ></Typed> */}
        </div>
        <button
          about="#"
          className="bg-[#1bd4f1] text-black w-40 mx-auto mt-6 rounded p-2"
        >
          {" "}
          Get Started
        </button>
      </div>
    </div>
  );
}

export default Home;

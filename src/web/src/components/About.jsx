import React from "react";
import about1 from "../assets/about.png"
import { Link } from "react-router-dom";

function About() {
  const styles = {
    line: "bg-inherit px-[10px] md:text-xl",
  }


  return (
    <>
      <h1 className="text-[#1bd4f1] text-center text-3xl md:text-5xl pb-16">About Us</h1>
      <div className="flex flex-col items-center justify-center w-full">
        <div className="flex flex-col justify-center pb-16 md:px-16 md:w-full md:flex-row xl:px-72">
          <div>
            <h2 className="pb-5 text-xl md:text-4xl text-[#1bd4f1] text-left px-[10px]">Welcome to Meta Vision</h2>
            <p className="pb-4 px-[10px] md:text-xl">Our website offers cutting-edge face detection technology that can analyze and identify faces in images with remarkable accuracy. Our state-of-the-art algorithms are designed to provide fast and reliable results, making it easier for you to extract information from images.</p>
          </div>
          <img src={about1} alt="ai techno" className="h-80 w-[500px] mx-auto md:w-[350px] lg:w-[500px] md:ml-1" />
        </div>
        <div className="w-full h-full py-16 bg-slate-700 md:px-16 xl:px-72">
          <h2 className="pb-5 text-xl md:text-4xl text-[#1bd4f1] text-left bg-inherit px-[10px]">Incoming Features</h2>
          <p className={styles.line}>We believe that the world of images and videos analysis is constantly evolving, and that's why we're always working to add new features and capabilities to our platform.</p>
          <p className={styles.line}>In the near future, you can expect to see a wide range of additional features, including :</p>
          <li className={styles.line}>The ability to detect other objects in images </li>
          <li className={styles.line}>Improve image quality, upscale videos </li>
          <li className={styles.line}>Perform face comparison tests</li>
          <li className={styles.line}>And even more ! </li>
          <p className={styles.line}>These exciting new capabilities will allow you to unlock even more valuable insights from your images and take your analysis to the next level.</p>
        </div>
        <div className="flex flex-col w-full pt-16 pb-20 md:px-16 xl:px-72">
          <h2 className="pb-5 text-xl md:text-4xl text-[#1bd4f1] text-left px-[10px]">Try Yourself</h2>
          <p className="px-[10px] pb-5 md:text-xl">So why wait? Whether you're a professional photographer, a researcher, or simply someone who loves to explore the world through images, our platform has something for you. Sign up today and start discovering the amazing insights that await you!</p>
          <button
            about="#"
            className="bg-[#1bd4f1] text-black w-40 mx-auto mt-6 rounded p-2 md:text-xl"
          >
            {" "}
            <Link to={{
              pathname: "/upload/new-upload",
            }}
              state={{ confirm: false }}
              className="text-black bg-[#1bd4f1]">
              Try it !
            </Link>
          </button>
        </div>
      </div>
    </>
  );
}

export default About;

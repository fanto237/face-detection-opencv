import React from "react";

function Footer() {
  return (
    <div className="flex items-center justify-center h-[5vh]">
      Source code available{" "}
      <a
        target="_blank"
        rel="noreferrer"
        href="https://github.com/fantoSama/face-recognize-opencv"
        className="ml-1 text-[#1bd4f1] underline"
      >
        here
      </a>
    </div>
  );
}

export default Footer;

import React from "react";

import conf from "../assets/firework.gif";



function Confirmation({ values }) {

  const styles = {
    important: "font-bold text-white text-[#1bd4f1]",
    parag: "",
  }

  const SubmitedValue = values;

  return (
    <div className="flex flex-col items-center justify-center">
      <p className="py-20 px-4 text-[#1bd4f1] text-center text-2sm sm:text-xl sm:px-6 md:text-2xl md:px-[370px]">Thank you <span className={styles.important}>{values.userName}</span> for submitting your order. Your order number is <span className={styles.important}>{values.orderId}</span>, you can use it to track your order and you will soon received an email for confirmation</p>
      <img src={conf} className="w-[350px] h-[300px] pb-10 sm:w-[550px] sm:h-[400px] md:w-[750px] md:h-[500px]"></img>
    </div>
  );
}

export default Confirmation;

import React from "react";



function Confirmation({ values }) {

  const SubmitedValue = values;

  return (<div className="flex flex-col items-center justify-center">
    <div><p>Thanks for submitting your order, you will soon received an email on the Adresse: {values.email} </p></div>
    <iframe src="https://giphy.com/embed/u4CY9BW4umAfu" className="w-[900px] h-[400px]" allowFullScreen></iframe>
  </div>);
}

export default Confirmation;

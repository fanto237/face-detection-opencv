import React, { useState, useEffect } from "react";

const defaultImageUrl = '/img/default.jpg';

const initialFieldValues = {
  orderId: 0,
  username: '',
  email: '',
  imageName: '',
  imageUrl: defaultImageUrl,
  imageFile: null
}

function Upload() {

  const [values, setValues] = useState(initialFieldValues);

  const handleInputChange = e => {
    const { name, value } = e.target;

    setValues({
      ...values,
      [name]: value,
    })
    console.log(values.username);
    console.log(values.email);
  }

  const showPreview = e => {
    if (e.target.files && e.target.files[0]) {
      let imageFile = e?.target.files[0];
      const reader = new FileReader();
      console.log(reader);
      console.log("+++++++++++++++");
      reader.onload = x => {
        console.log("entering the onload")
        console.log(x.target.result);

        setValues({
          ...values,
          imageFile,
          ImageUrl: x.target.result,
        })
      }
      reader.readAsDataURL(imageFile);
      console.log(reader);
    }
  }


  return (
    <div className="flex flex-col items-center justify-center my-10 mx-auto sm:my-[100px]">
      <div >
        <img className="w-[350px] h-[250px] sm:h-[450px] sm:w-[550px] md:h-[550px] md:w-[700px]" src={values.imageUrl} alt="default selected image" />
      </div>
      <div className="my-4">
        <form action="#" className="flex flex-col items-center justify-center" autoComplete="off" noValidate>
          <input type="file" accept="image/*" name="imageFile" className=" text-[16px]  bg-white text-black rounded-[10px] outline-none my-1 sm:my-3 w-72 sm:w-[400px]" value={values.imageFile} onChange={showPreview} />
          <input type="text" className="my-1 border-[1px] sm:my-3 sm:w-[400px] w-72 h-8 rounded-[10px]   bg-slate-50" placeholder="enter your name" name="userName" value={values.username} onChange={handleInputChange} />
          <input type="text" className="my-1 w-72 sm:my-3 sm:w-[400px] h-8 border-[1px] rounded-[10px]  bg-slate-50" placeholder="enter your email" name="email" value={values.email} onChange={handleInputChange} />
        </form>
      </div>
    </div>
  );
}

export default Upload;

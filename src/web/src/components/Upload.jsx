import React, { useState } from "react";

function Upload() {

  const defaultImageUrl = '/img/default.jpg';


  const initialFieldValues = {
    orderId: crypto.randomUUID(),
    userName: '',
    email: '',
    imageName: '',
    imageUrl: defaultImageUrl,
    imageFile: null
  }

  const [values, setValues] = useState(initialFieldValues);
  const handleInputChange = e => {
    const { name, value } = e.target;
    setValues({
      ...values,
      [name]: value,
    })
  }

  const [errors, setErrors] = useState({})

  const showPreview = e => {
    if (e.target.files && e.target.files[0]) {  // if the files array contained in the event e is set and its first value is valid 
      let imgFile = e?.target.files[0]; // then store it in the imgFile variable
      const reader = new FileReader();
      reader.onload = x => {

        setValues({
          ...values,
          imageFile: imgFile,
          imageUrl: x.target.result,
        })
      }
      reader.readAsDataURL(imgFile);
    } else {
      setValues({
        ...values,
        imageFile: null,
        imageUrl: defaultImageUrl,
      })
    }
  }

  const Validate = () => {
    const temp = {};
    temp.userName = values.userName === "" ? false : true;
    temp.email = values.email === "" ? false : true;
    temp.imageUrl = values.imageUrl === defaultImageUrl ? false : true;
    setErrors(temp);

    // return temp.email && temp.name && temp.imageUrl;
    return Object.values(temp).every(x => x === true);
    // Object.value will return all elements of temp 
    // then with every the iterate through thoses elements and test if all of them are true => if so then the whole instruction if true
  }

  const handleFormSubmit = e => {
    e.preventDefault();
    if (Validate()) {

    }
  }

  const applyErrorStyle = (field) => {

    if (field in errors && errors[field] === false) {
      console.log("the form is not properly filed for the entry " + field);
      return " bg-red text-red border-red-50";
    } else {
      console.log("everything is just fine for the entry " + field);
      return '';
    }
    // " bg-red text-red border-red-50" : "")

  }

  return (
    <div className="flex flex-col items-center justify-center my-10 mx-auto sm:my-[100px]">
      <div >
        <img className="w-[350px] h-[250px] sm:h-[450px] sm:w-[550px] md:h-[550px] md:w-[700px]" src={values.imageUrl} alt="selected" />
      </div>
      <div className="my-4">
        <form onSubmit={handleFormSubmit} className="flex flex-col items-center justify-center" autoComplete="off" noValidate>
          <input type="file" className={"text-[16px] bg-white text-black rounded-[10px] outline-none my-1 sm:my-3 w-72 sm:w-[400px]" + applyErrorStyle('imageUrl')} name="imageFile" accept="image/*" onChange={e => showPreview(e)} />
          <input type="text" className={"my-1 border-[1px] sm:my-3 px-1 sm:w-[400px] w-72 h-8 rounded-[10px] text-black bg-slate-50" + applyErrorStyle('userName')} placeholder="enter your name" name="userName" value={values.userName} onChange={handleInputChange} />
          <input type="text" className={"my-1 w-72 sm:my-3 sm:w-[400px] px-1 h-8 border-[1px]ds rounded-[10px]  text-black bg-slate-50" + applyErrorStyle('email')} placeholder="enter your email" name="email" value={values.email} onChange={handleInputChange} />
          <button className="bg-[#1bd4f1] py-2 px-3 rounded-[10px] mt-4" type="submit"> Submit Order </button>
        </form>
      </div>
    </div>
  );
}

export default Upload;

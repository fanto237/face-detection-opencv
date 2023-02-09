import React, { useEffect, useState } from "react";
import { BiSearch } from "react-icons/bi";
import instance from "../api/endPoint";
import Order from "./Order";

function Status() {

  const style = {
    div1: "flex flex-col items-center justify-center w-full",
    title: "text-[#1bd4f1] text-center text-3xl md:text-5xl py-10",
    input: "border-[white] border-2 rounded  text-center py-1 w-[320px] sm:w-96",
    button: "mt-2 sm:mt-0 sm:ml-2 py-[2px] px-2 rounded bg-[#1bd4f1] flex  items-center",

  }
  const [resp, setResp] = useState(null);



  const [input, setInput] = useState("");
  const [hasFetched, setHasFetched] = useState(false);
  const [image, SetImage] = useState(null);

  const deserializeToImage = (byteArray) => {
    // Create a blob from the byte array
    const blob = new Blob([byteArray], { type: 'image/jpeg' });

    // Create an object URL from the blob
    const objectURL = URL.createObjectURL(blob);

    // Create an image element
    const img = new Image();

    // Set the src of the image to the object URL
    img.src = objectURL;

    // Return the image element
    console.log("the image is: " + img);
  }

  useEffect(() => {
    console.log("I got fired");
    // setHasFetched(true);
  }, [resp])


  const handleOnclick = () => {
    // e.preventDefault();
    console.log("i am in the handler");
    instance.get("/api/orders/" + input)
      .then((response) => {
        console.log(response.data);
        // setResponse(response.data);
        // console.log(response);
        setResp(response.data);
        // console.log(resp);

      })
      .then(() => {
        console.log(resp);
      })
      .catch(err => {
        console.log("the error ist :" + err);
      })
    // try {
    //   const resp = aw instance.get("/api/orders/" + input);
    //   console.log(resp);
    //   // setResponse(resp.data);
    //   if (resp.data === null) {
    //     console.log("the response is null")
    //   } else {
    //     console.log("the hasbeenFetched before :" + hasFetched)
    //     setHasFetched(true);
    //     console.log("the hasbeenFetched after :" + hasFetched)
    //     deserializeToImage(response.imageData);
    //   }
    // } catch (err) {
    //   console.log("the error is : " + err);
    // }
  }

  // useEffect(() => {
  //   console.log("i am from the use effect");
  // }, [])
  // console.log(input);

  return (
    <> {
      hasFetched
        ?
        <div className={style.div1}>
          <h1 className={style.title}>
            Track your order
          </h1>
          <div className={style.div1 + " py-10 sm:flex-row"}>
            <input type="text" value={input} placeholder="Enter your order number" className={style.input} onChange={e => setInput(e.target.value)} />
            <button className={style.button} onClick={handleOnclick}><BiSearch size={28} className="bg-[#1bd4f1]" /><span className="pl-1 uppercase text-2sm bg-inherit">Track</span></button>
          </div>
          <Order response={resp} image={image} />
        </div>
        :
        <div className={style.div1}>
          <h1 className={style.title}>
            Track your order
          </h1>
          <div className={style.div1 + " py-10 sm:flex-row"}>
            <input type="text" value={input} placeholder="Enter your order number" className={style.input} onChange={e => setInput(e.target.value)} />
            <button className={style.button} onClick={handleOnclick}><BiSearch size={28} className="bg-[#1bd4f1]" /><span className="pl-1 uppercase text-2sm bg-inherit">Track</span></button>
          </div>
        </div>
    }
    </>
  )
}

export default Status;

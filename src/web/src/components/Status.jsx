import React, { useState } from "react";
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
  const [result, setResult] = useState(null);



  const [input, setInput] = useState("");
  const [hasFetched, setHasFetched] = useState(false);

  const valid = (id) => {
    if (id === "")
      return false;
    const regex = "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";
    const result = id.match(regex);
    return result === null ? false : true;
  }


  const handleOnclick = async (e) => {
    e.preventDefault();
    console.log("i am in the handler");
    if (valid(input)) {
      console.log("the order id is correct")
      try {
        const resp = await instance.get("/api/orders/" + input);
        console.log(resp);
        setResult(resp.data);
        if (resp.data === null) {
          alert("There is no order corresponding to this number, try again with a correct number")
        } else {
          console.log("the hasbeenFetched before :" + hasFetched)
          setHasFetched(true);
          console.log("the hasbeenFetched after :" + hasFetched)
        }
      } catch (err) {
        console.log("the error is : " + err);
      }

    } else {
      alert("the order entered was not a valid order id, please try with a correct number");
      setInput("");
      setHasFetched(false);
    }
  }


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
          <div className="w-full">
            <Order result={result} />
          </div>
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

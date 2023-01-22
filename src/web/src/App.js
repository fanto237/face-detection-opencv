import React from "react";
import Home from "./components/Home";
import Navbar from "./components/Navbar";
import Footer from "./components/Footer";
import Status from "./components/Status";
import Usage from "./components/Usage";
import Upload from "./components/Upload";
import About from "./components/About";
import Confirmation from "./components/Confirmation";
import { BrowserRouter, Routes, Route } from "react-router-dom";

function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <div className="min-h-[80vh]">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route exact path="/status" element={<Status />} />
          <Route exact path="/confirmation" element={<Confirmation />} />
          <Route exact path="/about" element={<About />} />
          <Route exact path="/upload" element={<Upload />} />
          <Route exact path="/terms-of-usage" element={<Usage />} />
        </Routes>
      </div>
      <Footer />
    </BrowserRouter>
  );
}

export default App;

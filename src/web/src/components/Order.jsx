import React from 'react'

function Order(props) {

    const resp = props.result;

    const getStatus = () => {
        if (resp.status === 0)
            return "Registered";
        else if (resp.status === 1)
            return "Processed";
        else if (resp.status === 2)
            return "Sent";
    }
    return (
        <div>
            <table className='m-auto '>
                <tr>
                    <th className='p-2 border text-[#1bd4f1]'>Image</th>
                    <th className='p-2 border text-[#1bd4f1]'>Order Id</th>
                    <th className='p-2 border text-[#1bd4f1]'>Username</th>
                    <th className='p-2 border text-[#1bd4f1]'>Email</th>
                    <th className='p-2 border text-[#1bd4f1]'>Status</th>
                </tr>
                <tr>
                    <td className='p-2 border'><img src={"data:image/jpeg;base64," + resp.imageData} alt="the is the best" className='w-32 h-40' /></td>
                    <td className='p-2 border'>{resp.orderId}</td>
                    <td className='p-2 border'>{resp.userName}</td>
                    <td className='p-2 border'>{resp.email}</td>
                    <td className='p-2 border'>{getStatus()}</td>
                </tr>
            </table>
        </div>
    )
}

export default Order
import React from 'react'

function Order(props) {
    const resp = props.result;

    const style = {
        table_header: 'p-2 border text-[#1bd4f1]',
        table_entry: 'p-2 border'
    }

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
                    <th className={style.table_header}>Image</th>
                    <th className={style.table_header}>Order Id</th>
                    <th className={style.table_header}>Username</th>
                    <th className={style.table_header}>Email</th>
                    <th className={style.table_header}>Status</th>
                </tr>
                <tr>
                    <td className={style.table_entry}><img src={"data:image/jpeg;base64," + resp.imageData} alt="the is the best" className='w-32 h-40' /></td>
                    <td className={style.table_entry}>{resp.orderId}</td>
                    <td className={style.table_entry}>{resp.userName}</td>
                    <td className={style.table_entry}>{resp.email}</td>
                    <td className={style.table_entry}>{getStatus()}</td>
                </tr>
            </table>
        </div>
    )
}

export default Order
import axios from 'axios'

const API_URL = 'https://api.example.com' // Replace with your actual API URL

export const cryptoService = {
  async getMarketData() {
    const response = await axios.get(`${API_URL}/crypto/market`)
    return response.data
  },

  async getWalletBalance() {
    const response = await axios.get(`${API_URL}/wallet/balance`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem('token')}`
      }
    })
    return response.data
  },

  async executeTrade(type: 'buy' | 'sell', crypto: string, amount: number) {
    const response = await axios.post(
      `${API_URL}/trade`,
      {
        type,
        crypto,
        amount
      },
      {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      }
    )
    return response.data
  }
}
<div align="center">

# Sync Puzzle

서로 다른 정보를 가진 두 플레이어가  
협동하여 퍼즐을 해결하는 2인 온라인 퍼즐 게임

<img width="1018" height="678" alt="image" src="https://github.com/user-attachments/assets/64bf2850-6698-4dab-b79c-53a43befb7e2" />
>

</div>

---

## 프로젝트 소개

Sync Puzzle은 플레이어마다 다른 정보와 시야를 기반으로  
협력하여 퍼즐을 해결하는 멀티플레이 퍼즐 게임입니다.

MasterClient 기반 퍼즐 판정 구조를 사용하여  
멀티플레이 환경에서 퍼즐 상태를 일관되게 유지하도록 구성했습니다.

---

## Links

<p align="center">
  <a href=["여기에_유튜브링크"](https://www.youtube.com/watch?v=iuVHFhuQx14)>
    <img src="https://img.shields.io/badge/YouTube-시연영상-red?style=for-the-badge&logo=youtube&logoColor=white"/>
  </a>
</p>

---

## 개발 정보

- **엔진** : Unity
- **언어** : C#
- **네트워크** : Photon PUN2
- **인증** : Firebase Authentication
- **플랫폼** : Windows PC
- **개발 인원** : 개인 프로젝트

---

## 멀티플레이 진행 흐름

<img width="711" height="411" alt="image" src="https://github.com/user-attachments/assets/e6c8cf4e-2861-4176-935a-0c68944072cf" />

---

## 주요 기능

### 1. MasterClient 기반 퍼즐 판정 구조

모든 퍼즐 입력과 판정을 MasterClient에서 처리하여  
클라이언트 간 퍼즐 상태 불일치 문제를 방지했습니다.

<img width="370" height="467" alt="image" src="https://github.com/user-attachments/assets/47872200-01c4-4982-b2b5-1f0a24d3e5ca" />

---

### 2. 협동 타이밍 퍼즐

두 플레이어가 일정 시간 안에 동시에 버튼을 눌러야 성공하는 퍼즐입니다.

- PhotonNetwork.Time 기반 시간 비교
- 입력 시간 저장 및 동기화
- 실패 시 퍼즐 상태 초기화

<img width="800" height="450" alt="ezgif-83a20dd030644743" src="https://github.com/user-attachments/assets/4dcf6d11-13bc-4e11-a0d3-0bb8889ddb5d" />

---

### 3. 비밀번호 입력 퍼즐

한 플레이어만 키패드를 조작할 수 있으며  
정답 입력 시 blocker가 해제됩니다.

- 입력 권한 관리
- 문자열 기반 정답 판정
- 실패 시 입력값 초기화

<img width="800" height="450" alt="ezgif-82552feed11c6c52" src="https://github.com/user-attachments/assets/beb84361-69ac-4b14-a17e-a48d8bc5b49d" />

---

### 4. 레버 조합 퍼즐

레버 입력 상태를 배열로 관리하여  
정해진 조합 만족 시 성공하는 퍼즐입니다.

- 레버 상태 배열 저장
- 조합 비교 기반 판정
- 실패 시 전체 초기화

<img width="800" height="450" alt="ezgif-853853a69ebea389" src="https://github.com/user-attachments/assets/abbcedec-467e-4932-97d3-b94fb0230260" />

---

### 5. 레이저 반사 퍼즐

거울을 회전시켜 레이저를 센서까지 유도하는 퍼즐입니다.

- Raycast2D 기반 충돌 계산
- Reflect 기반 반사 방향 처리
- 거울 회전 결과 RPC 동기화

<img width="800" height="450" alt="ezgif-89953feab253e196" src="https://github.com/user-attachments/assets/299d60d5-ba6a-4921-8d97-378bb5ad9917" />

---

### 6. 로프 협동 퍼즐

두 플레이어를 로프로 연결하고  
일정 거리 이상 멀어지면 서로를 끌어당기는 퍼즐입니다.

- LineRenderer 기반 로프 시각화
- Rigidbody2D 기반 힘 적용
- 거리 기반 견인 처리

<img width="800" height="450" alt="ezgif-8e41eae2e1d8a127" src="https://github.com/user-attachments/assets/bb611a99-e95f-4658-ab41-2c8dbe3904d8" />

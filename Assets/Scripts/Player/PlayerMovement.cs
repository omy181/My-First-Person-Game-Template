using System.Collections;
using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        public Transform GroundCheck;
        [Header("Properties")]
        public float MaxVelocity;
        public float VelocityCoefficientTillLimit;
        public float VelocityChangeLimit;
        public float VelocityCoefficient;
        public float JumpCoefficient;
        public float FallGravityCoefficient;
        public float GroundDrag;
        public float MaxVelDrag;
        public float AirDrag;
        public float DefaultDrag;
        public float AirVelocityCoefficient;

        public int JumpCount;
        int jumpcount;
        public int Getjumpcount() { return jumpcount; }
        bool jumpready = true;
        public delegate void JumpDelegate(int jumpcount);
        public JumpDelegate JumpSignal;

        [Space]
        public bool isMovement = true;
        public bool isRigidboyFrozen;

        void Update()
        {
            // Get Inputs
            bool isJump = Input.GetKeyDown(KeyCode.Space) && isMovement;
            float Hinput = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
            float Vinput = Input.GetAxisRaw("Vertical") * Time.deltaTime;

            // Jump
            if (isJump && jumpready)
            {
                if (isOnGround())
                {
                    Vector3 JumpForce = transform.up * JumpCoefficient;
                    GetComponent<Rigidbody>().AddForce(JumpForce, ForceMode.Impulse);

                    StartCoroutine(JumpCoolDown());
                }
                else if (jumpcount > 0)
                {
                    jumpcount--;
                    Vector3 JumpForce = transform.up * JumpCoefficient;
                    GetComponent<Rigidbody>().AddForce(JumpForce, ForceMode.Impulse);

                    StartCoroutine(JumpCoolDown());
                }
            }
            if (isOnGround() && jumpready && jumpcount != JumpCount)
            {
                jumpcount = JumpCount;
            }

            // Gravity
            GravityMultiplier();

            // Force Movement
            Vector3 MovementForce;
            if (isOnGround())
            {
                if (GetComponent<Rigidbody>().velocity.magnitude < VelocityChangeLimit)
                {
                    Vinput *= VelocityCoefficientTillLimit;
                    Hinput *= VelocityCoefficientTillLimit;
                }
                else
                {
                    Vinput *= VelocityCoefficient;
                    Hinput *= VelocityCoefficient;
                }

                MovementForce = transform.forward.normalized * Vinput + transform.right.normalized * Hinput;
            }
            else
            {
                Vinput *= VelocityCoefficient;
                Hinput *= VelocityCoefficient;

                MovementForce = transform.forward.normalized * Vinput * AirVelocityCoefficient + transform.right.normalized * Hinput * AirVelocityCoefficient;
            }

            // Movement On/Off
            if (!isMovement) { MovementForce = Vector3.zero; }

            GetComponent<Rigidbody>().AddForce(MovementForce);

            // Drag Control
            GetComponent<Rigidbody>().drag = SetDrag(MovementForce);


        }


        float SetDrag(Vector3 MovementForce)
        {
            if (MovementForce.magnitude == 0 && isOnGround())
                return GroundDrag;

            if (GetComponent<Rigidbody>().velocity.magnitude >= MaxVelocity)
                return MaxVelDrag;

            if (!isOnGround())
                return AirDrag;

            return DefaultDrag;
        }
        public bool isOnGround()
        {
            Collider[] hits = Physics.OverlapSphere(GroundCheck.position, 0.4f);

            foreach (Collider hit in hits)
            {
                if (hit.gameObject.layer == 6)
                {
                    return true;
                }
            }

            return false;
        }


        IEnumerator JumpCoolDown()
        {
            JumpSignal(jumpcount);

            jumpready = false;
            yield return new WaitForSeconds(0.1f);
            jumpready = true;
        }

        void GravityMultiplier()
        {
            if (GetComponent<Rigidbody>().velocity.y < 0)
                GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * Time.deltaTime * FallGravityCoefficient;
        }

        public void SwitchRigidbodyLock()
        {
            if (isRigidboyFrozen)
            {

                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                isRigidboyFrozen = false;
            }
            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                isRigidboyFrozen = true;


            }

        }
    }

